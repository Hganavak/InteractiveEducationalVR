using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRTK;


public class StripedShellfish : VRTK_InteractableObject
{
	public SceneManagerIslandHMD sceneManager;
	public Slider progressBar;
	public AudioSource islandLectureShellfishFound;


	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		Debug.Log (PlayerPrefs.GetInt ("progress"));
		SceneManagerIslandHMD.shellfishFound = true;
		StartCoroutine ("playIslandLectureShellfishFound");
	}

	IEnumerator playIslandLectureShellfishFound() {
		sceneManager.DisableMovement ();
		sceneManager.UpdateProgress (5); sceneManager.UpdateScore (50);
		islandLectureShellfishFound.Play (); //Play a short clip saying that the object has been found
		yield return new WaitForSeconds (islandLectureShellfishFound.clip.length); //Wait until lecture's finished before disabling the shell, and enabling movement
		ForceReleaseGrab (); //Force user to drop the shellfish and then make it so it can't be picked up
		isUsable = false;
		isGrabbable = false;
		sceneManager.StartCoroutine ("PlayIslandLecturePlantsIntroduction");
	}

}