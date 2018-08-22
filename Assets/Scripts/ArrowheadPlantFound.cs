using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ArrowheadPlantFound : VRTK_InteractableObject {

	public SceneManagerIslandHMD sceneManager;
	public AudioSource islandLectureArrowheadFound;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		if (!SceneManagerIslandHMD.arrowheadPlantFound) { //If an arrowhead plant hasn't already been found
			SceneManagerIslandHMD.arrowheadPlantFound = true;
			Debug.Log ("You found an arrowhead plant");
			isUsable = false;
			touchHighlightColor = Color.clear; //Disable highlighting the object
			StartCoroutine ("playIslandLectureArrowheadFound");
		}
	}

	IEnumerator playIslandLectureArrowheadFound() {
		islandLectureArrowheadFound.Play(); //Play a short clip saying that the object has been found
		yield return new WaitForSeconds(islandLectureArrowheadFound.clip.length); // Wait until clip has finished before checking if all trees have been found
		sceneManager.checkIfAllPlantsFound();
	}

}
