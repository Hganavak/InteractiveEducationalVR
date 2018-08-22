using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerForestPC : SceneManagerPC {

	public PortalMasterPC pm;

	//Shooting
	private RaycastHit objectHit;

	//Lectures
	public AudioSource forestLectureIntroductionAndTaskDescription;
	public AudioSource forestLectureAllTrapsFoundAndBerryTaskDescription;
	public AudioSource forestLectureIncorrectChoice;
	public AudioSource forestLectureCorrectChoice;

	//Game variables
	private const int NUMBER_OF_RAT_TRAPS = 3;
	private const int TIME_TO_FIND_RAT_TRAPS = 60; //1 minutes
	private bool ratTrap1Found; public GameObject ratTrap1Light;
	private bool ratTrap2Found; public GameObject ratTrap2Light;
	private bool ratTrap3Found; public GameObject ratTrap3Light;
	private int numberOfTrapsFound;
	private bool allRatTrapsFound;


	public GameObject berries;
	private bool whiteBerryTried;
	private bool blackBerryTried;
	private bool crimsonBerryTried;



	// Use this for initialization
	IEnumerator Start () {
		Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;

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
	
	//Shooting Checks
	void Update()
	{
		if (Input.GetMouseButton (0)) {
			Debug.DrawRay (player.transform.position, player.transform.forward * interactionDistance, Color.blue);

			if (Physics.Raycast (player.transform.position, player.transform.forward, out objectHit, interactionDistance)) {
				//Debug.Log ("You shot: " + objectHit.transform.name);
				//Do all the checks
				if (objectHit.transform.name == "Rat Trap 1" && !ratTrap1Found) {
					Debug.Log ("You found rat trap 1");
					ratTrap1Found = true;
					ratTrap1Light.SetActive (false);
					ratTrapFound ();
				} else if (objectHit.transform.name == "Rat Trap 2" && !ratTrap2Found) {
					Debug.Log ("You found rat trap 2");
					ratTrap2Found = true;
					ratTrap2Light.SetActive (false);
					ratTrapFound ();
				} else if (objectHit.transform.name == "Rat Trap 3" && !ratTrap3Found) {
					Debug.Log ("You found rat trap 3");
					ratTrap3Found = true;
					ratTrap3Light.SetActive (false);
					ratTrapFound ();
				} else if (objectHit.transform.name == "White Berry" && !whiteBerryTried) {
					whiteBerryTried = true;
					Debug.Log ("You found the white berry");
					StartCoroutine (incorrectBerryChosen ());
				}
				else if (objectHit.transform.name == "Black Berry" && !blackBerryTried) {
					blackBerryTried = true;
					Debug.Log ("You found the black berry");
					StartCoroutine (incorrectBerryChosen ());
				}
				else if (objectHit.transform.name == "Crimson Berry" && !crimsonBerryTried) {
					crimsonBerryTried = true;
					Debug.Log ("You found the crimson berry");
					StartCoroutine (CorrectBerryChosen ());
				}

			} 
		}
	}//End Update()

	//Light up a rat trap if it hasn't been found after 1.5 minutes
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

	private IEnumerator CorrectBerryChosen() {
		UpdateScore (30); UpdateProgress (11);

		DisableMovement ();

		pm.EnablePortals();

		forestLectureCorrectChoice.Play ();
		yield return new WaitForSeconds (forestLectureCorrectChoice.clip.length);


		EnableMovement ();

		yield return null;
	}

	private IEnumerator incorrectBerryChosen() {
		UpdateScore (-30);

		interactionDistance = 0;
		DisableMovement ();

		forestLectureIncorrectChoice.Play ();
		yield return new WaitForSeconds (forestLectureIncorrectChoice.clip.length);

		EnableMovement ();
		interactionDistance = 3;

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
}
