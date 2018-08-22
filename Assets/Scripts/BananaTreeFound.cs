using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BananaTreeFound : VRTK_InteractableObject {

	public SceneManagerIslandHMD sceneManager;
	public AudioSource islandLectureBananaTreeFound;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		if (!SceneManagerIslandHMD.bananaTreeFound) { //If a banana tree hasn't already been found
			SceneManagerIslandHMD.bananaTreeFound = true;
			Debug.Log ("You found a bananana tree");
			isUsable = false;
			touchHighlightColor = Color.clear; //Disable highlighting the object
			StartCoroutine ("playIslandLectureBananaTreeFound");
		}
	}

	IEnumerator playIslandLectureBananaTreeFound() {
		islandLectureBananaTreeFound.Play(); //Play a short clip saying that the object has been found
		yield return new WaitForSeconds(islandLectureBananaTreeFound.clip.length); // Wait until clip has finished before checking if all trees have been found
		sceneManager.checkIfAllPlantsFound();
	}

}
