using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RubbleFound : VRTK_InteractableObject {

	public SceneManagerMountainHMD sceneManager;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		sceneManager.rubbleFound ();
		Debug.Log ("You found some rubble");
		isUsable = false;
		touchHighlightColor = Color.clear; //Disable highlighting the object
	}
}
