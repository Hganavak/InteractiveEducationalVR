using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BerryUsed : VRTK_InteractableObject {

	public SceneManagerForestHMD sceneManager;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		Debug.Log ("You used a berry");
		isUsable = false;
		touchHighlightColor = Color.clear; //Disable highlighting the object

		StartCoroutine(sceneManager.BerryUsed (this.gameObject));

	}
}
