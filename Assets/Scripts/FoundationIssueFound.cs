using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class FoundationIssueFound : VRTK_InteractableObject {

	public SceneManagerRoomHMD sceneManager;
	public AudioSource roomLectureFoundationIssueFound;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		if (!sceneManager.foundationIssueFound) { //If a foundation issue hasn't already been found
			sceneManager.foundationIssueFound = true;
			sceneManager.DisableMovement ();
			Debug.Log ("You found a foundation issue");
			isUsable = false;
			touchHighlightColor = Color.clear; //Disable highlighting the object
			StartCoroutine ("playRoomLectureFoundationIssueFound");
		}
	}

	IEnumerator playRoomLectureFoundationIssueFound() {
		roomLectureFoundationIssueFound.Play(); //Lecture that explains issues with the building's foundations
		yield return new WaitForSeconds(roomLectureFoundationIssueFound.clip.length); // Wait until clip has finished before checking if all trees have been found
		sceneManager.checkIfAllIssuesFound();
	}

}
