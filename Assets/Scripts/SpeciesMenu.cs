using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeciesMenu : MonoBehaviour
{
	public Transform previousAnimal;
	private TextMeshProUGUI previousAnimalText;
	private Vector2 previousAnimalPosition;

	public Transform currentAnimal;
	private TextMeshProUGUI currentAnimalText;
	private Vector2 currentAnimalPosition;
	
	public Transform nextAnimal;
	private TextMeshProUGUI nextAnimalText;
	private Vector2 nextAnimalPosition;
	public AnimationCurve switchCurve = AnimationCurve.EaseInOut(0,0,1,1);
	private int currentMoveWay = 0;
	private float switchTime=0;
	private float canvasWidth=0;
	private void Awake()
	{
		previousAnimalText = previousAnimal.GetComponentInChildren<TextMeshProUGUI>();
		currentAnimalText = currentAnimal.GetComponentInChildren<TextMeshProUGUI>();
		nextAnimalText = nextAnimal.GetComponentInChildren<TextMeshProUGUI>();
		previousAnimalPosition = previousAnimal.transform.localPosition;
		currentAnimalPosition = currentAnimal.transform.localPosition; 
		nextAnimalPosition = nextAnimal.transform.localPosition;
		canvasWidth = GetComponent<RectTransform>().rect.width;
		// TODO (priorité faible):
		//       - Ajouter une image à chaque espèce (nom à lire dans csv, fichier à lire dans StreamingAssets (mais pas à faire dans cette classe a priori))
		//       - Récupérer le bouton (enfant de currentAnimal) et s'abonner au OnClick
		//       - Créer une méthode a appeler sur le OnClick qui "fait sortir" le canvas du sol pour le mettre
		//         à la vertical et faire apparaitre un menu
		//       - Faire une méthode appeler quand on click sur un élément du menu, fait la transition vers l'espèce choisie
	}   //       - Faire une méthode qui "fait revenir" le canvas sur le sol et réaffiche l'espèce choisie
	private void Update()
	{
		if(currentMoveWay<0)
		{
			switchTime += Time.deltaTime;
			if(switchTime > 1)
			{
				Set(previousAnimalText.text);
				return;
			}
			previousAnimal.transform.localPosition = previousAnimalPosition - switchCurve.Evaluate(switchTime) * canvasWidth * Vector2.right;
			currentAnimal.transform.localPosition = currentAnimalPosition - switchCurve.Evaluate(switchTime) * canvasWidth * Vector2.right;
		}
		if (currentMoveWay>0)
		{
			switchTime += Time.deltaTime;
			if (switchTime > 1)
			{
				Set(nextAnimalText.text);
				return;
			}
			nextAnimal.transform.localPosition = nextAnimalPosition + switchCurve.Evaluate(switchTime) * canvasWidth * Vector2.right;
			currentAnimal.transform.localPosition = currentAnimalPosition + switchCurve.Evaluate(switchTime) * canvasWidth * Vector2.right;
		}
	}
	public void Set(string animalName)
	{
		currentAnimalText.text = animalName;
		currentMoveWay = 0;
		switchTime = 0;
		previousAnimal.transform.localPosition = previousAnimalPosition;
		currentAnimal.transform.localPosition = currentAnimalPosition;
		nextAnimal.transform.localPosition = nextAnimalPosition;
	}
	public void SwitchPrevious(string animalName)
	{
		previousAnimalText.text = animalName;
		currentMoveWay = -1;
		switchTime = 0;
	}
	public void SwitchNext(string animalName)
	{
		nextAnimalText.text = animalName;
		currentMoveWay = 1;
		switchTime = 0;
	}
}
