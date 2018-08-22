using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RatTrapFound : VRTK_InteractableObject {

	public SceneManagerForestHMD sceneManager;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		sceneManager.ratTrapFound ();
		Debug.Log ("You used a rat trap");
		isUsable = false;
		touchHighlightColor = Color.clear; //Disable highlighting the object

		if (this.gameObject.transform.name == "Rat Trap 1") {
			sceneManager.ratTrap1Found = true;
		} else if (this.gameObject.transform.name == "Rat Trap 2") {
			sceneManager.ratTrap2Found = true;
		} else if (this.gameObject.transform.name == "Rat Trap 3") {
			sceneManager.ratTrap3Found = true;
		}

	}
}
