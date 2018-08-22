using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class SouthWestCornerActivator : VRTK_InteractableObject {

	public SceneManagerMemoryPalaceSkyHMD sceneManager;
	public Slider progressBar;
	public AudioSource ding;
	public GameObject cornerLight;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		Debug.Log ("You used the button");
		sceneManager.southWestActivatorUsed = true;
		sceneManager.UpdateScore(50); sceneManager.UpdateProgress(5);
		cornerLight.SetActive (false);
		sceneManager.CheckIfGameOver ();
	}
}
