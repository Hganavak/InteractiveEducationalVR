using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TutorialTarget : VRTK_InteractableObject {

	public SceneManagerTutorialHMD sceneManager;
	public AudioSource ding;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		Debug.Log("You clicked on the target");

		ding.Play ();
		pointerActivatesUseAction = false;
		isUsable = false;
		sceneManager.CheckIfAllTargetsFound ();
		Destroy (this.gameObject);
	}
}
