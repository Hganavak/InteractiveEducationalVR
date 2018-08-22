using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class SceneManagerForestHMD : SceneManagerHMD {

	//Lectures
	public AudioSource forestLectureIntroductionAndTaskDescription;
	public AudioSource forestLectureAllTrapsFoundAndBerryTaskDescription;
	public AudioSource forestLectureIncorrectChoice;
	public AudioSource forestLectureCorrectChoice;

	//Game variables
	private const int NUMBER_OF_RAT_TRAPS = 3;
	private const int TIME_TO_FIND_RAT_TRAPS = 60; //1 minutes
	public bool ratTrap1Found; public GameObject ratTrap1Light;
	public bool ratTrap2Found; public GameObject ratTrap2Light;
	public bool ratTrap3Found; public GameObject ratTrap3Light;
	private int numberOfTrapsFound;
	private bool allRatTrapsFound;

	public GameObject berries;
	private bool whiteBerryTried;
	private bool blackBerryTried;
	private bool crimsonBerryTried;


	IEnumerator Start() {
		PlayerPrefs.SetInt ("forestVisited", 1);

		//Set initial progress bar
		UpdateProgress ();
		DisableMovement ();

		//Introduction lecture
		forestLectureIntroductionAndTaskDescription.Play();
		yield return new WaitForSeconds(forestLectureIntroductionAndTaskDescription.clip.length);

		EnableMovement ();
		Invoke ("EnableRatTrapLights", TIME_TO_FIND_RAT_TRAPS);

		yield return null;
	}

	public void ratTrapFound() {
		numberOfTrapsFound++;
		UpdateProgress (3); UpdateScore (30);
		if (numberOfTrapsFound >= NUMBER_OF_RAT_TRAPS) {
			allRatTrapsFound = true;
			StartCoroutine (PlayForestLectureAllTrapsFoundAndBerryTaskDescription());
		}
	}

	private IEnumerator PlayForestLectureAllTrapsFoundAndBerryTaskDescription() {
		DisableMovement ();
		forestLectureAllTrapsFoundAndBerryTaskDescription.Play ();
		yield return new WaitForSeconds (forestLectureAllTrapsFoundAndBerryTaskDescription.clip.length);

		berries.SetActive (true);
		EnableMovement ();
		yield return null;
	}

	//Light up a rat trap if it hasn't been found after TIME_TO_FIND_RAT_TRAPS
	private void EnableRatTrapLights() {
		if (!allRatTrapsFound) {
			if (!ratTrap1Found)
				ratTrap1Light.SetActive (true);
			if(!ratTrap2Found)
				ratTrap2Light.SetActive (true);
			if(!ratTrap3Found)
				ratTrap3Light.SetActive (true);
		}
	}

	public IEnumerator BerryUsed(GameObject berry) {
		if (berry.name == "Black Berry" || berry.name == "White Berry") {
			UpdateScore (-30);
			forestLectureIncorrectChoice.Play ();
		} else if (berry.name == "Crimson Berry") {
			UpdateScore (30); UpdateProgress (11);
			DisableMovement ();

			NextLevel ();

			forestLectureCorrectChoice.Play ();
			yield return new WaitForSeconds (forestLectureCorrectChoice.clip.length);

			EnableMovement ();

		}
		yield return null;
	}
		
	public void NextLevel() {
		pm.UpdatePortalStates ();

		GameObject portals = GameObject.Find ("LevelPortals");
		portals.transform.position = GameObject.Find("Location for Portals to Appear").transform.position;
		portals.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
	}

}
