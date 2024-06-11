using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SpeciesSwitcher : MonoBehaviour
{
	public Material postProcess;
	public List<Texture3D> textures;
	public List<string> names;
	public List<BloomParams[]> bloomParams=new List<BloomParams[]>();
	public List<float[]> postExposure=new List<float[]>();
	public TMP_Text animals_text;
	public LightmapSwitcher lightmapSwitcher;
	public Texture3D humanTexture;

	public InputActionReference Menu;
	private bool inTransition = false;
	public float textureBlend = 0;
	public int indexTexture0 = 0;
	public int indexTexture1 = 0;
	public Volume volume;
	private Bloom bloom;
	private ColorAdjustments colorAdjustments;
	public float bloomInitialIntensity;
	public Color bloomInitialTint;
	[Serializable]
	public class BloomParams
	{
		public float intensity;
		public float scatter;
		public Color tint;
	}
	public SpeciesMenu menu;
	private void Start()
	{
		postProcess.SetTexture("_Texture0", textures[0]);
		postProcess.SetTexture("_Texture1", textures[1]);
		volume.profile.TryGet<Bloom>(out bloom);
		volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
		bloomInitialIntensity = bloom.intensity.value;
		bloomInitialTint = bloom.tint.value;
		menu.Set(names[0]);
	}

	private void Update()
	{

		if (Menu.action.triggered)
			Debug.Log(Menu.action.ReadValue<Vector2>());
		if (!inTransition && (Menu.action.triggered && Menu.action.ReadValue<Vector2>().x > 0 || Input.GetKeyDown(KeyCode.RightArrow)))
		{
			inTransition = true;
			indexTexture1 = (indexTexture0 + 1) % textures.Count;
			postProcess.SetTexture("_Texture1", textures[indexTexture1]);
			menu.SwitchNext(names[indexTexture1]);
		}
		else if (!inTransition && (Menu.action.triggered && Menu.action.ReadValue<Vector2>().x < 0 || Input.GetKeyDown(KeyCode.LeftArrow)))
		{
			inTransition = true;
			indexTexture1 = (indexTexture0 - 1) % textures.Count;
			if (indexTexture1<0)
				indexTexture1 += textures.Count;
			postProcess.SetTexture("_Texture1", textures[indexTexture1]);
			menu.SwitchPrevious(names[indexTexture1]);
		}
		if (inTransition)
		{
			//textureBlend += Time.deltaTime;
			textureBlend = 2; // use this line instead of the above line to transition instantly to the next texture
			if(textureBlend>1)
			{
				textureBlend = 0;
				postProcess.SetFloat("_Blend", textureBlend);
				indexTexture0 = indexTexture1;
				postProcess.SetTexture("_Texture0", textures[indexTexture0]);
				postProcess.SetTexture("_Texture1", textures[indexTexture1]);
				ApplyPostProcess();
				inTransition = false;
				return;
			}
			bloom.intensity.value = Mathf.Lerp(bloomParams[indexTexture0][lightmapSwitcher.current].intensity, bloomParams[indexTexture1][lightmapSwitcher.current].intensity, textureBlend);
			bloom.scatter.value = Mathf.Lerp(bloomParams[indexTexture0][lightmapSwitcher.current].scatter, bloomParams[indexTexture1][lightmapSwitcher.current].scatter, textureBlend);
			bloom.tint.value = Color.Lerp(bloomParams[indexTexture0][lightmapSwitcher.current].tint, bloomParams[indexTexture1][lightmapSwitcher.current].tint, textureBlend);
			colorAdjustments.postExposure.value = Mathf.Lerp(postExposure[indexTexture0][lightmapSwitcher.current], postExposure[indexTexture1][lightmapSwitcher.current], textureBlend);
			postProcess.SetFloat("_Blend", textureBlend);
		}

		//DisplayAnimalsName();
	}

	public void ApplyPostProcess(){
		bloom.intensity.value = bloomParams[indexTexture0][lightmapSwitcher.current].intensity;
		bloom.scatter.value = bloomParams[indexTexture0][lightmapSwitcher.current].scatter;
		bloom.tint.value = bloomParams[indexTexture0][lightmapSwitcher.current].tint;
		colorAdjustments.postExposure.value = postExposure[indexTexture0][lightmapSwitcher.current];
	}

	public void SetHumanTexture()
	{
		postProcess.SetFloat("_Blend", 0);
		postProcess.SetTexture("_Texture0", textures[0]);
		postProcess.SetTexture("_Texture1", textures[0]);

		bloom.intensity.value = bloomParams[0][lightmapSwitcher.current].intensity;
		bloom.scatter.value = bloomParams[0][lightmapSwitcher.current].scatter;
		bloom.tint.value = bloomParams[0][lightmapSwitcher.current].tint;
		colorAdjustments.postExposure.value = postExposure[0][lightmapSwitcher.current];

		animals_text.text = names[0];
	}

	public void SetBatTexture()
	{
		postProcess.SetFloat("_Blend", 1);
		postProcess.SetTexture("_Texture0", textures[1]);
		postProcess.SetTexture("_Texture1", textures[1]);

		bloom.intensity.value = bloomParams[1][lightmapSwitcher.current].intensity;
		bloom.scatter.value = bloomParams[1][lightmapSwitcher.current].scatter;
		bloom.tint.value = bloomParams[1][lightmapSwitcher.current].tint;
		colorAdjustments.postExposure.value = postExposure[1][lightmapSwitcher.current];

		animals_text.text = names[1];
	}

	public void SetAnoureTexture()
	{
		postProcess.SetFloat("_Blend", 2);
		postProcess.SetTexture("_Texture0", textures[2]);
		postProcess.SetTexture("_Texture1", textures[2]);

		bloom.intensity.value = bloomParams[2][lightmapSwitcher.current].intensity;
		bloom.scatter.value = bloomParams[2][lightmapSwitcher.current].scatter;
		bloom.tint.value = bloomParams[2][lightmapSwitcher.current].tint;
		colorAdjustments.postExposure.value = postExposure[2][lightmapSwitcher.current];

		animals_text.text = "Grenouille";
	}

	public void SetPassereauTexture()
	{
		postProcess.SetFloat("_Blend", 3);
		postProcess.SetTexture("_Texture0", textures[3]);
		postProcess.SetTexture("_Texture1", textures[3]);

		bloom.intensity.value = bloomParams[3][lightmapSwitcher.current].intensity;
		bloom.scatter.value = bloomParams[3][lightmapSwitcher.current].scatter;
		bloom.tint.value = bloomParams[3][lightmapSwitcher.current].tint;
		colorAdjustments.postExposure.value = postExposure[3][lightmapSwitcher.current];

		animals_text.text = names[3];
	}



	private void OnDestroy()
	{
		if (textures.Count > 0)
    	{
        postProcess.SetTexture("_Texture0", textures[0]);
   		}
	}

	void DisplayAnimalsName()
	{
		for (int i = 0; i < names.Count; i++)
		{
			if (postProcess.GetTexture("_Texture0").name == textures[i].name)
			{
				animals_text.text = names[i];
			}
		}
		
	}
}