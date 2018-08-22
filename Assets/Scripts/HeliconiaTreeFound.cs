using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class HeliconiaTreeFound : VRTK_InteractableObject {

	public SceneManagerIslandHMD sceneManager;
	public AudioSource islandLectureHeliconiaFound;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		if (!SceneManagerIslandHMD.heliconiaTreeFound) { //If a heliconia tree hasn't already been found
			SceneManagerIslandHMD.heliconiaTreeFound = true;
			Debug.Log ("You found a heliconia tree");
			isUsable = false;
			touchHighlightColor = Color.clear; //Disable highlighting the object
			StartCoroutine ("playIslandLectureHeliconiaFound");
		}
	}

	IEnumerator playIslandLectureHeliconiaFound() {
		islandLectureHeliconiaFound.Play(); //Play a short clip saying that the object has been found
		yield return new WaitForSeconds(islandLectureHeliconiaFound.clip.length); // Wait until clip has finished before checking if all trees have been found
		sceneManager.checkIfAllPlantsFound();
	}

}
