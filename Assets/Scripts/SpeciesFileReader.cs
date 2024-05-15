using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SpeciesFileReader : MonoBehaviour
{
	public static readonly string fileName = "AnimalsData.csv";
	private static readonly int NB_AMBIANCE = 5;
	public SpeciesSwitcher speciesSwitcher;
	public Texture3D humanTexture;

	private List<SpeciesData> species;
	
	private void Start()
	{
		species = ReadAnimalsFile(fileName);
		LoadTextures3D(species);
		SetPostProcessParams();
		speciesSwitcher.ApplyPostProcess();
	}
	class SpeciesData
	{
		public string name;
		public Spectrum spectrumData;
		public Spectrum spectrumGain;
		public SpeciesSwitcher.BloomParams bloomParams = new SpeciesSwitcher.BloomParams();
		public SpeciesSwitcher.BloomParams[] bloomParamsByAmbiance = {
			new SpeciesSwitcher.BloomParams(),
			new SpeciesSwitcher.BloomParams(),
			new SpeciesSwitcher.BloomParams(),
			new SpeciesSwitcher.BloomParams(),
			new SpeciesSwitcher.BloomParams()
		};
		public float exposureParam;
		public float[] exposureParamByAmbiance = new float[5];
	}
	private void SetPostProcessParams()
	{
		speciesSwitcher.bloomParams.Clear();
		speciesSwitcher.postExposure.Clear();

		foreach (var data in species)
		{
			speciesSwitcher.bloomParams.Add(data.bloomParamsByAmbiance);
			speciesSwitcher.postExposure.Add(data.exposureParamByAmbiance);
		}
	}

	private static Spectrum CalculateSpeciesGains(SpeciesData humainData, SpeciesData data)
	{
		var humain = humainData.spectrumData;
		humain.Trim();
		var humanInterval = humain.GetInterval();
		Spectrum speciesSpectrum = data.spectrumData;
			
		speciesSpectrum.Trim();
		var specieInterval = speciesSpectrum.GetInterval();
		var newInterval = ((specieInterval.Item2 - specieInterval.Item1) / (humanInterval.Item2 - humanInterval.Item1));

		for (int i = 0; i < speciesSpectrum.wavelengths.Length; i++)
		{
			speciesSpectrum.wavelengths[i] = speciesSpectrum.wavelengths[i] - specieInterval.Item1;
			speciesSpectrum.wavelengths[i] = speciesSpectrum.wavelengths[i] / newInterval;
			speciesSpectrum.wavelengths[i] = humanInterval.Item1 + speciesSpectrum.wavelengths[i];
		}

		speciesSpectrum = speciesSpectrum.Resampled(humain.wavelengths);
		Spectrum gainSpecie = speciesSpectrum / humain;
		return gainSpecie;
	}

	private class AnimalsFileStructure
	{
		public const int speciesNameIndex = 0;
		public int startWavelengthsIndex = -1;
		public int bloomIntensityIndex = -1;
		public int bloomScatterIndex = -1;
		public int bloomTintIndex = -1;
		public int exposureIndex = -1;
		public int[] bloomIntensityByAmbianceIndexes = new int[5];
		public int[] bloomScatterByAmbianceIndexes = new int[5];
		public int[] bloomTintByAmbianceIndexes = new int[5];
		public int[] exposureByAmbianceIndexes = new int[5];
	}
	private static List<SpeciesData> ReadAnimalsFile(string fileName)
	{
		if (string.IsNullOrEmpty(fileName))
			return null;

		var animalsFile = Path.Combine(Application.streamingAssetsPath, fileName);
		if (!animalsFile.ToLower().EndsWith(".csv"))
		{
			animalsFile += ".csv";
		}
#if !UNITY_ANDROID
		if (!File.Exists(animalsFile))
		{
			Debug.LogError("The Animals File \"" + animalsFile + "\" is missing. Please add it to the StreamingAssets/Scenarios folder and start again.");
			return null;
		}
		var reader = new StreamReader(new FileStream(animalsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
#else


		UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(animalsFile);
		www.SendWebRequest();
		while (!www.isDone)
		{
		}
		if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
		{
			Debug.LogError("Unable to read file "+animalsFile +" with error: "+www.result);
			return null;
		}
		//Debug.Log("Read in file "+scenarioFile+":"+www.downloadedBytes);
		var reader = new StreamReader(new MemoryStream(www.downloadHandler.data));
#endif
		var header = CSVReader.ReadCSVLine(reader, ';');
		while (header.Length == 0 || header[0].StartsWith('#') || string.IsNullOrEmpty(header[0]))
			header = CSVReader.ReadCSVLine(reader, ';');
		List<float> wavelengthsList = new List<float>();
		AnimalsFileStructure fileStruct = new AnimalsFileStructure();
		
		ReadHeader(header, wavelengthsList, fileStruct);
		
		Spectrum.defaultWaveLengths = wavelengthsList.ToArray();
		var valeur = CSVReader.ReadCSVLine(reader, ';');

		List<SpeciesData> species = new List<SpeciesData>();
		SpeciesData human = null;
		while (valeur != null)
		{
			if (valeur.Length == 0 || valeur[0].StartsWith('#') || string.IsNullOrEmpty(valeur[0]))
				continue;
			if (valeur.Length != Spectrum.defaultWaveLengths.Length +fileStruct.startWavelengthsIndex)
			{
				Debug.LogError("pas assez d'elements dans le tableau");
				valeur = CSVReader.ReadCSVLine(reader, ';');
				continue;
			}
			float[] animalsData = new float[Spectrum.defaultWaveLengths.Length];
			for (int i = fileStruct.startWavelengthsIndex; i < Spectrum.defaultWaveLengths.Length + fileStruct.startWavelengthsIndex; i++)
			{
				animalsData[i - fileStruct.startWavelengthsIndex] = float.Parse(valeur[i], CultureInfo.InvariantCulture);
			}
			var specieSpectrum = new Spectrum(Spectrum.defaultWaveLengths, animalsData);
			var specieData = new SpeciesData();
			specieData.name = valeur[0];
			specieData.spectrumData = specieSpectrum;
			if (species.Count == 0)
				human = specieData;
			specieData.spectrumGain = CalculateSpeciesGains(human, specieData);
			ReadPostProcessParams(specieData, fileStruct, valeur);

			species.Add(specieData);
			valeur = CSVReader.ReadCSVLine(reader, ';');
		}
		return species;
	}

	private static void ReadHeader(string[] header, List<float> wavelengthsList, AnimalsFileStructure fileStruct)
	{
		for (int i = 1; i < header.Length; i++)
		{
			float waveLength;
			if (fileStruct.startWavelengthsIndex<=0)
			{
				if (header[i].ToLower().Trim() == "bloom intensity")
				{
					fileStruct.bloomIntensityIndex = i;
				}
				else if (header[i].ToLower().Trim() == "bloom scatter")
				{
					fileStruct.bloomScatterIndex = i;
				}
				else if (header[i].ToLower().Trim() == "bloom tint")
				{
					fileStruct.bloomTintIndex = i;
				}
				else if (header[i].ToLower().Trim() == "exposure")
				{
					fileStruct.exposureIndex = i;
				}
				else if (header[i].ToLower().StartsWith("bloom intensity"))
				{
					var indexStr = header[i].Substring("bloom intensity".Length).Trim();
					int scene;
					if (int.TryParse(indexStr, out scene))
					{
						fileStruct.bloomIntensityByAmbianceIndexes[scene] = i;
					}
				}
				else if (header[i].ToLower().StartsWith("bloom scatter"))
				{
					var indexStr = header[i].Substring("bloom scatter".Length).Trim();
					int scene;
					if (int.TryParse(indexStr, out scene))
					{
						fileStruct.bloomScatterByAmbianceIndexes[scene] = i;
					}
				}
				else if (header[i].ToLower().StartsWith("bloom tint"))
				{
					var indexStr = header[i].Substring("bloom tint".Length).Trim();
					int scene;
					if (int.TryParse(indexStr, out scene))
					{
						fileStruct.bloomTintByAmbianceIndexes[scene] = i;
					}
				}
				else if (header[i].ToLower().StartsWith("exposure"))
				{
					var indexStr = header[i].Substring("exposure".Length).Trim();
					int scene;
					if (int.TryParse(indexStr, out scene))
					{
						fileStruct.exposureByAmbianceIndexes[scene] = i;
					}
				}
				else if (float.TryParse(header[i], out waveLength))
				{
					wavelengthsList.Add(waveLength);
					fileStruct.startWavelengthsIndex = i;
				}
				else
				{
					Debug.LogWarning("Champ "+header[i]+" inconnu.");
				}
				continue;
			}
			if (float.TryParse(header[i], out waveLength))
			{
				wavelengthsList.Add(waveLength);
			}
			else
			{
				Debug.LogWarning("Le champ "+header[i]+" devrait �tre une colonne situ�e avant les longueurs d'ondes.");
			}
		}
	}

	private static void ReadPostProcessParams(SpeciesData specieData, AnimalsFileStructure fileStruct, string[] valeur)
	{
		Color defaultTint = Color.black;
		float defaultIntensity = 0;
		for (int i = 0; i< specieData.spectrumGain.wavelengths.Length; i++)
		{
			defaultTint+=WavelengthColor.GetColorFromWaveLength(specieData.spectrumGain.wavelengths[i])*specieData.spectrumGain.values[i];
			defaultIntensity+=specieData.spectrumGain.values[i];
		}
		defaultTint/=defaultIntensity;
		defaultIntensity /= specieData.spectrumGain.wavelengths.Length;
		if (fileStruct.bloomIntensityIndex>0 && !string.IsNullOrEmpty(valeur[fileStruct.bloomIntensityIndex]))
		{
			float val;
			if (float.TryParse(valeur[fileStruct.bloomIntensityIndex], NumberStyles.Any, CultureInfo.InvariantCulture, out val))
			{
				specieData.bloomParams.intensity = val;
			}
			else
			{
				Debug.LogError("bloomIntensity invalide: "+valeur[fileStruct.bloomIntensityIndex]);
			}
		}
		else
		{
			specieData.bloomParams.intensity = defaultIntensity;
		}
		if (fileStruct.bloomScatterIndex>0 && !string.IsNullOrEmpty(valeur[fileStruct.bloomScatterIndex]))
		{
			float val;
			if (float.TryParse(valeur[fileStruct.bloomScatterIndex], NumberStyles.Any, CultureInfo.InvariantCulture, out val))
			{
				specieData.bloomParams.scatter = val;
			}
			else
			{
				Debug.LogError("bloomScatter invalide: "+valeur[fileStruct.bloomScatterIndex]);
			}
		}
		else
		{
			specieData.bloomParams.scatter = specieData.bloomParams.intensity / 10;
		}
		if (fileStruct.bloomTintIndex>0 && !string.IsNullOrEmpty(valeur[fileStruct.bloomTintIndex]))
		{
			specieData.bloomParams.tint = ParseColor(valeur[fileStruct.bloomTintIndex]);
		}
		else
		{
			specieData.bloomParams.tint = defaultTint;
		}
		if (fileStruct.exposureIndex>0 && !string.IsNullOrEmpty(valeur[fileStruct.exposureIndex]))
		{
			float val;
			if (float.TryParse(valeur[fileStruct.exposureIndex], NumberStyles.Any, CultureInfo.InvariantCulture, out val))
			{
				specieData.exposureParam = val;
			}
			else
			{
				Debug.LogError("bloomIntensity invalide: "+valeur[fileStruct.bloomIntensityIndex]);
			}
		}
		else
		{
			specieData.exposureParam = 0;
		}
		for (int i = 0;i<NB_AMBIANCE;i++)
		{
			if (fileStruct.bloomIntensityByAmbianceIndexes[i]>0&& !string.IsNullOrEmpty(valeur[fileStruct.bloomIntensityByAmbianceIndexes[i]]))
			{
				float val;
				if (float.TryParse(valeur[fileStruct.bloomIntensityByAmbianceIndexes[i]], NumberStyles.Any, CultureInfo.InvariantCulture, out val))
				{
					specieData.bloomParamsByAmbiance[i].intensity = val;
				}
				else
				{
					Debug.LogError("bloomIntensity "+i+" invalide: "+valeur[fileStruct.bloomIntensityByAmbianceIndexes[i]]);
				}
			}
			else
			{
				specieData.bloomParamsByAmbiance[i].intensity = specieData.bloomParams.intensity;
			}
			if (fileStruct.bloomScatterByAmbianceIndexes[i]>0&& !string.IsNullOrEmpty(valeur[fileStruct.bloomScatterByAmbianceIndexes[i]]))
			{
				float val;
				if (float.TryParse(valeur[fileStruct.bloomScatterByAmbianceIndexes[i]], NumberStyles.Any, CultureInfo.InvariantCulture, out val))
				{
					specieData.bloomParamsByAmbiance[i].scatter = val;
				}
				else
				{
					Debug.LogError("bloomScatter "+i+" invalide: "+valeur[fileStruct.bloomScatterByAmbianceIndexes[i]]);
				}
			}
			else
			{
				specieData.bloomParamsByAmbiance[i].scatter = specieData.bloomParams.scatter;
			}
			if (fileStruct.bloomTintByAmbianceIndexes[i]>0&& !string.IsNullOrEmpty(valeur[fileStruct.bloomTintByAmbianceIndexes[i]]))
			{
				specieData.bloomParamsByAmbiance[i].tint = ParseColor(valeur[fileStruct.bloomTintByAmbianceIndexes[i]]);
			}
			else
			{
				specieData.bloomParamsByAmbiance[i].tint = specieData.bloomParams.tint;
			}
			if (fileStruct.exposureByAmbianceIndexes[i]>0&& !string.IsNullOrEmpty(valeur[fileStruct.exposureByAmbianceIndexes[i]]))
			{
				float val;
				if (float.TryParse(valeur[fileStruct.exposureByAmbianceIndexes[i]], NumberStyles.Any, CultureInfo.InvariantCulture, out val))
				{
					specieData.exposureParamByAmbiance[i] = val;
				}
				else
				{
					Debug.LogError("exposure "+i+" invalide: "+valeur[fileStruct.bloomIntensityByAmbianceIndexes[i]]);
				}
			}
			else
			{
				specieData.exposureParamByAmbiance[i] = specieData.exposureParam;
			}
		}
	}

	public static Color ParseColor(string hex)
	{
		if (hex.Length<6)
		{
			throw new System.FormatException("Needs a string with a length of at least 6");
		}

		var r = hex.Substring(0, 2);
		var g = hex.Substring(2, 2);
		var b = hex.Substring(4, 2);
		string alpha;
		if (hex.Length >= 8)
			alpha = hex.Substring(6, 2);
		else
			alpha = "FF";

		return new Color((int.Parse(r, NumberStyles.HexNumber) / 255f),
						(int.Parse(g, NumberStyles.HexNumber) / 255f),
						(int.Parse(b, NumberStyles.HexNumber) / 255f),
						(int.Parse(alpha, NumberStyles.HexNumber) / 255f));
	}
	private void LoadTextures3D(List<SpeciesData> species)
	{
		speciesSwitcher.names.Clear();
		speciesSwitcher.textures.Clear();
		speciesSwitcher.names.Add(species[0].name);
		speciesSwitcher.textures.Add(humanTexture);
		for (int i=1;i<species.Count;i++)
		{
			var e = species[i];
			Texture3D tex3d=Texture3DIO.Read(e.name);
			if(tex3d == null)
			{
				tex3d = CreateTexture3D(e.name, e.spectrumGain);
				// Save the texture to your file
				//Texture3DIO.Write(e.Key, tex3d);
			}
			//Ajouter tex3d au speciesSwitcher.textures
			speciesSwitcher.textures.Add(tex3d);
			//Ajouter le nom de l'esp�ce au speciesSwitcher.names
			speciesSwitcher.names.Add(e.name);
			//Penser � supprimer les textures et noms des esp�ces au speciesSwitcher dans la scene (laisser l'Humain)
		}
	}
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			Start();
		}
	}
	private static Texture3D CreateTexture3D(string key, Spectrum gain)
	{
		// Configure the texture
		int size = 32;
		TextureFormat format = TextureFormat.RGBA32;
		TextureWrapMode wrapMode = TextureWrapMode.Clamp;

		// Create the texture and apply the configuration
		Texture3D texture = new Texture3D(size, size, size, format, false);
		texture.name = key;
		texture.wrapMode = wrapMode;

		// Create a 3-dimensional array to store color data
		Color[] colors = new Color[size * size * size];

		// Populate the array so that the x, y, and z values of the texture will map to red, blue, and green colors
		float inverseResolution = 1.0f / (size - 1.0f);
		for (int z = 0; z < size; z++)
		{
			int zOffset = z * size * size;
			for (int y = 0; y < size; y++)
			{
				int yOffset = y * size;
				for (int x = 0; x < size; x++)
				{
					colors[x + yOffset + zOffset] = new Color(x * inverseResolution,
						y * inverseResolution, z * inverseResolution, 1.0f);
					Spectrum s = Spectrum.FromColor(colors[x + yOffset + zOffset]);
					s = s * gain;
					colors[x + yOffset + zOffset] = s.ToColor();
				}
			}
		}

		// Copy the color values to the texture
		texture.SetPixels(colors);

		// Apply the changes to the texture and upload the updated texture to the GPU
		texture.Apply();
		return texture;
	}
}
