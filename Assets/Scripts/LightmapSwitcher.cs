
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.IO;

public class LightmapSwitcher : MonoBehaviour {

	static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
	{
		// Get information about the source directory
		var dir = new DirectoryInfo(sourceDir);

		// Check if the source directory exists
		if (!dir.Exists)
			throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

		// Cache directories before we start copying
		DirectoryInfo[] dirs = dir.GetDirectories();

		// Create the destination directory
		Directory.CreateDirectory(destinationDir);

		// Get the files in the source directory and copy to the destination directory
		foreach (FileInfo file in dir.GetFiles())
		{
			string targetFilePath = Path.Combine(destinationDir, file.Name);
			if (targetFilePath.EndsWith(".meta"))
				continue;
			file.CopyTo(targetFilePath, true);
		}

		// If recursive and copying subdirectories, recursively call this method
		if (recursive)
		{
			foreach (DirectoryInfo subDir in dirs)
			{
				string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
				CopyDirectory(subDir.FullName, newDestinationDir, true);
			}
		}
	}
	[System.Serializable]
	public class SphericalHarmonics
	{
		public float[] coefficients = new float[27];
	}
	[System.Serializable]
	public class MaterialSwitch {
		public Material material;
		public List<Renderer> renderers;
		public int materialIndex=-1;
	}
	[System.Serializable]
	public class MaterialChangeEmmissive
	{
		public Material[] materials;
		public bool activeEmissive;
	}
	[System.Serializable]
	public class LightingData {
		public string name;
		[Tooltip("Path to the lightmaps (relative to Resource directory)")]
		public string path;
		
		public Material skybox;
		[Tooltip("GameObjects to activate")]
		public List<GameObject> toActivate;
		[Tooltip("GameObjects to desactivate")]
		public List<GameObject> toDesactivate;
		[Tooltip("Change material of renderers")]
		public List<MaterialSwitch> renderers;
		[Tooltip("Active/Desactive emissive on materials")]
		public List<MaterialChangeEmmissive> materialEmissivesChange;
		public List<SphericalHarmonics> lightProbes;
		
		internal LightmapData[] lightmapData;
		internal Texture[] reflections;
		[InspectorButton("SaveCurrentLightmaps")]
		public bool saveCurrentLightMaps;
		public void SaveCurrentLightmaps()
		{
			if(string.IsNullOrEmpty(path))
				path="Lightmaps/"+name;
			var ressourceLMPath = Path.Combine(Application.dataPath,"Resources/"+path);
			if (!Directory.Exists(ressourceLMPath))
			{
				Directory.CreateDirectory(ressourceLMPath);
			}
			var scenePath = SceneManager.GetActiveScene().path;
			var currentLMPath= Path.Combine(Path.GetDirectoryName(scenePath), Path.GetFileNameWithoutExtension(scenePath));
			CopyDirectory(currentLMPath, ressourceLMPath, true);
		}

		[InspectorButton("SaveCurrentLightProbes")]
		public bool saveCurrentLightProbes;
		public void SaveCurrentLightProbes() {
			lightProbes.Clear();
			var scene_LightProbes = LightmapSettings.lightProbes.bakedProbes;

			for (int i = 0; i < scene_LightProbes.Length; i++)
			{
				var SHCoeff = new SphericalHarmonics();

				// j is coefficient
				for (int j = 0; j < 3; j++)
				{
					//k is channel ( r g b )
					for (int k = 0; k < 9; k++)
					{
						SHCoeff.coefficients[j*9+k] = scene_LightProbes[i][j, k];
					}
				}

				lightProbes.Add(SHCoeff);
			}
		}
		[InspectorButton("Set")]
		public bool set;
		public void Set()
		{
			if (Application.isPlaying)
			{
				Instance.SwapLightmaps(name);
			}
			else
			{
				Instance.ChangeSceneObjectsAndLightsParams(name);
#if UNITY_EDITOR
				CopyLightmapsToCurrent();
#endif
			}
		}
#if UNITY_EDITOR
		public void CopyLightmapsToCurrent()
		{
			path="Lightmaps/"+name;
			var ressourceLMPath = Path.Combine(Application.dataPath, "Resources/"+path);
			if (!Directory.Exists(ressourceLMPath))
			{
				return;
			}
			var scenePath = SceneManager.GetActiveScene().path;
			var currentLMPath = Path.Combine(Path.GetDirectoryName(scenePath), Path.GetFileNameWithoutExtension(scenePath));
			CopyDirectory(ressourceLMPath, currentLMPath, true);
			UnityEditor.AssetDatabase.Refresh();
		}
#endif
	}

	void ChangeSceneObjectsAndLightsParams(string name) {
		var d = data.First(o=>o.name == name);
		if(d != null) {
			if (d.skybox)
			{
				RenderSettings.skybox = d.skybox;
			}
			foreach (var ri in d.renderers) {
				foreach(var r in ri.renderers) {
					if (r)
					{
						if (ri.materialIndex <= 0)
						{
							r.sharedMaterial = ri.material;
						}
						else
						{
							var mats = r.sharedMaterials;
							mats[ri.materialIndex] = ri.material;
							r.sharedMaterials = mats;
						}
					}
				}
			}
			foreach (var o in d.materialEmissivesChange)
			{
				foreach (var m in o.materials)
				{
					if (o.activeEmissive)
					{
						m.EnableKeyword("_EMISSION");
						m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
					}
					else
					{
						m.DisableKeyword("_EMISSION");
						m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
					}
				}
			}
			foreach (var o in d.toDesactivate) {
				if(o)
					o.SetActive(false);
			}
			foreach (var o in d.toActivate)
			{
				if(o)
					o.SetActive(true);
			}
		}
		
	}

	public List<LightingData> data;
	public int current = 0;
	public bool setCurrentAtStart = true;
	private ReflectionProbe[] allReflectionProbes;
	//path to the night lightmap and reflection substitudes
	//public string nightpath = "";

	private static LightmapSwitcher instance;
	public static LightmapSwitcher Instance {
		get
		{
			if(instance == null)
			{
				if (Application.isPlaying)
					return FindObjectOfType<LightmapSwitcher>();
				instance = FindObjectOfType<LightmapSwitcher>();
			}
			return instance;
		}
		private set
		{
			instance = value;
		}
	}

	public string CurrentName {
		get
		{
			return data[current].name;
		}
	}

	void Awake() {
		Instance = this;
	}
	[ContextMenu("Previous")]
	public void Previous()
	{
		current--;
		if(current < 0)
			current = data.Count-1;
		SwapLightmaps(data[current].name);
	}
	[ContextMenu("Next")]
	public void Next()
	{
		current++;
		if (current >= data.Count)
			current = 0;
		SwapLightmaps(data[current].name);
	}
	// Use this for initialization
	void Start () {

		//var currentData = data[current];

		foreach (var d in data) {
			//if(d == currentData) {
			//	continue;
			//}
			d.lightmapData = new LightmapData[LightmapSettings.lightmaps.Length];
			for (int i = 0; i < LightmapSettings.lightmaps.Length; i++) {
				d.lightmapData[i] = new LightmapData();
				d.lightmapData[i].lightmapColor = Resources.Load(d.path + "/" + LightmapSettings.lightmaps[i].lightmapColor.name) as Texture2D;
				if(LightmapSettings.lightmaps[i].lightmapDir)
					d.lightmapData[i].lightmapDir = Resources.Load(d.path + "/" + LightmapSettings.lightmaps[i].lightmapDir.name) as Texture2D;
				if(LightmapSettings.lightmaps[i].shadowMask)
					d.lightmapData[i].shadowMask = Resources.Load(d.path + "/" + LightmapSettings.lightmaps[i].shadowMask.name) as Texture2D;
					
			}
		}
		List<ReflectionProbe> probes = new List<ReflectionProbe>(FindObjectsOfType<ReflectionProbe>());
		for(int i=0;i<probes.Count;) {
			if(probes[i].mode == ReflectionProbeMode.Baked) {
				i++;
			} else {
				probes.RemoveAt(i);
			}
		}
		allReflectionProbes = probes.ToArray();
		foreach(var d in data) {
			d.reflections = new Texture[allReflectionProbes.Length];
			for (int i = 0; i < allReflectionProbes.Length; i++ )
			{
				if (allReflectionProbes[i].bakedTexture)
					d.reflections[i] = Resources.Load(d.path + "/" + allReflectionProbes[i].bakedTexture.name) as Texture;
				else if (allReflectionProbes[i].customBakedTexture)
					d.reflections[i] = Resources.Load(d.path + "/" + allReflectionProbes[i].customBakedTexture.name) as Texture;
				else
					Debug.LogError("Reflection Probe "+allReflectionProbes[i]+" baked texture is null...", this);
			}
		}
		/*foreach(var rp in allReflectionProbes)
		{
			var tex = rp.bakedTexture;
			rp.mode = ReflectionProbeMode.Custom;
			rp.customBakedTexture = tex;
		}*/
		if(setCurrentAtStart)
		{
			SwapLightmaps(data[current].name);
		}
	}

	public void SwapLightmaps (string name) {
		var d = data.First(o=>o.name == name);
		if(d == null)
			return;
		LightmapSettings.lightmaps = d.lightmapData;
		for (int i = 0; i < allReflectionProbes.Length; i++)
		{
			allReflectionProbes[i].customBakedTexture = d.reflections[i];
			allReflectionProbes[i].bakedTexture = d.reflections[i];
		}
		var scene_LightProbes = new SphericalHarmonicsL2[LightmapSettings.lightProbes.bakedProbes.Length];
		scene_LightProbes = LightmapSettings.lightProbes.bakedProbes;

		for (int i = 0; i < scene_LightProbes.Length; i++)
		{
			var SHCoeff = d.lightProbes[i];

			// j is coefficient
			for (int j = 0; j < 3; j++)
			{
				//k is channel ( r g b )
				for (int k = 0; k < 9; k++)
				{
					scene_LightProbes[i][j, k] = SHCoeff.coefficients[j*9+k];
				}
			}

		}
		LightmapSettings.lightProbes.bakedProbes = scene_LightProbes;
		ChangeSceneObjectsAndLightsParams(name);
		current = data.IndexOf(d);
	}
}
