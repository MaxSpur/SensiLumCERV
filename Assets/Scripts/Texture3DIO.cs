using System;
using System.IO;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public static class Texture3DIO
{
	public static Texture3D Read(string name)
	{
		// Vérifier la présence d'un fichier name+".tex3d" dans streaming assets
		// Si il n'existe pas
		//     Retourner null
		var file = Path.Combine(Application.streamingAssetsPath, name + ".tex3d");
		if (!File.Exists(file))
			return null;
		// Ouvrir le fichier name+".tex3d" dans streaming assets
		BinaryReader br = new BinaryReader(File.OpenRead(file));
		// Lire width, height, depth dans le fichier (3 ints)
		int width = br.ReadInt32();
		int height = br.ReadInt32();
		int depth = br.ReadInt32();
		// Creer un tableau de Color de width*height*depth éléments
		Color[] colors = new Color[width * height * depth];
		// Pour chaque elements du tableau
		for(int i = 0; i < colors.Length;i++)
		{
			// Lire r, g, b dans le fichier (3 floats)
			float r = br.ReadSingle();
			float g = br.ReadSingle();
			float b = br.ReadSingle();
			// Affecter un Color(r,g,b) à l'élément du tableau
			colors[i]=new Color(r, g, b);

		}
		// Fermer le fichier
		br.Close();
		// Creer une Texture3D(width,height,depth)
		var tex = new Texture3D(width, height, depth, TextureFormat.RGB24, false);
		// Affecter le tableau de Color aux pixels de la texture
		tex.SetPixels(colors);
		tex.Apply();
		// Retourner la texture
		return tex;
	}
	public static void Write(string name, Texture3D tex)
	{
		// Creer un fichier name+".tex3d" dans streaming assets
		var file = Path.Combine(Application.streamingAssetsPath, name + ".tex3d");
		BinaryWriter br = new BinaryWriter(File.OpenWrite(file));
		// Ecrire value.width,value.height, value.depth dans le fichier
		br.Write(tex.width);
		br.Write(tex.height);
		br.Write(tex.depth);
		Color[] colors = tex.GetPixels();
		// Pour chaque couleur dans data
		for (int i = 0; i < colors.Length; i++)
		{
			// Ecrire r, g, b (3 floats)
			br.Write(colors[i].r);
			br.Write(colors[i].g);
			br.Write(colors[i].b);
		}
		// Fermer le fichier
		br.Close();
	}
}