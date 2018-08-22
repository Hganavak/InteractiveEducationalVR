using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DoorFound : VRTK_InteractableObject {

	public SceneManagerRoomHMD sceneManager;
	public AudioSource roomLectureDoorFound;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		if (!sceneManager.doorFound) { //If a banana tree hasn't already been found
			sceneManager.doorFound = true;
			sceneManager.DisableMovement ();
			Debug.Log ("You found the door");
			isUsable = false;
			touchHighlightColor = Color.clear; //Disable highlighting the object
			StartCoroutine ("playRoomLectureDoorFound");
		}
	}

	IEnumerator playRoomLectureDoorFound() {
		roomLectureDoorFound.Play(); //Lecture that explains how the door was left unlocked
		yield return new WaitForSeconds(roomLectureDoorFound.clip.length); // Wait until clip has finished before checking if all trees have been found
		sceneManager.checkIfAllIssuesFound();
	}

}
