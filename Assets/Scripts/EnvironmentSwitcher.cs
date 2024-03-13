using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class EnvironmentSwitcher : MonoBehaviour
{
	public LightmapSwitcher lightmapSwitcher;
	public SpeciesSwitcher speciesSwitcher;
	public InputActionReference Menu;
	public TextMeshProUGUI envName;
	private void Start()
	{
		if (envName)
			envName.text = lightmapSwitcher.CurrentName;
	}
	private void Update()
	{
		if (Menu.action.triggered && Menu.action.ReadValue<Vector2>().y > 0 || Input.GetKeyDown(KeyCode.UpArrow))
		{
			lightmapSwitcher.Next();
			speciesSwitcher?.ApplyPostProcess();
			if(envName)
				envName.text = lightmapSwitcher.CurrentName;
		}
		else if (Menu.action.triggered && Menu.action.ReadValue<Vector2>().y < 0 || Input.GetKeyDown(KeyCode.DownArrow))
		{
			lightmapSwitcher.Previous();
			speciesSwitcher?.ApplyPostProcess();
			if (envName)
				envName.text = lightmapSwitcher.CurrentName;
		}
	}
}
