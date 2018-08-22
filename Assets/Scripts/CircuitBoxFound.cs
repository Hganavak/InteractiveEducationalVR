using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CircuitBoxFound : VRTK_InteractableObject {

	public SceneManagerRoomHMD sceneManager;
	public AudioSource roomLectureCircuitBoxFound;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		if (!sceneManager.circuitBoxFound) { //If a circuit box hasn't already been found
			sceneManager.circuitBoxFound = true;
			sceneManager.DisableMovement ();
			Debug.Log ("You found the circuit box");
			isUsable = false;
			touchHighlightColor = Color.clear; //Disable highlighting the object
			StartCoroutine ("playRoomLectureCircuitBoxFound");
		}
	}

	IEnumerator playRoomLectureCircuitBoxFound() {
		roomLectureCircuitBoxFound.Play(); //Lecture that explains the electrical fault
		yield return new WaitForSeconds(roomLectureCircuitBoxFound.clip.length); // Wait until clip has finished before checking if all trees have been found
		sceneManager.checkIfAllIssuesFound();
	}

}
