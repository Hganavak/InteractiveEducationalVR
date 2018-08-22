using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class SceneManagerTutorialHMD : MonoBehaviour {

	public GameObject player;

	public VRTK_Pointer leftController;
	public VRTK_Pointer rightController;
	public Slider progressBar;

	public PortalMaster pm;

	//Lectures
	public AudioSource tutorialLectureWelcomeGameAndMovementExplanation;

	public AudioSource tutorialLectureMovementTutorialHelp;
	public AudioSource tutorialLectureMovementTutorialComplete;

	public AudioSource tutorialLectureInteractionTutorialHelp;
	public AudioSource tutorialLectureInteractionTutorialComplete;

	public AudioSource tutorialLectureLaserPointerExplanation;

	public AudioSource tutorialLecturePortalExplanation;

	public AudioSource ding;

	//Game variables
	public static bool movementTutorialCompleted;
	public const int TIME_TO_COMPLETE_MOVEMENT_TUTORIAL = 20; //20 seconds

	public static bool interactionTutorialBegun;
	public bool tutorialButtonPressed;
	public bool interactionTutorialCompleted;
	public const int TIME_TO_COMPLETE_INTERACTION_TUTORIAL = 30; //30 seconds

	public static bool laserPointerTutorialBegun;
	public GameObject targets;
	public LaserPointer laserPointer;
	public int numberOfTargetsFound;
	public const int NUMBER_OF_TARGETS = 5;



	// Use this for initialization
	IEnumerator Start () {

		UpdateProgress (5);
		EnableMovement ();

		//Lecture: Welcome to game, game explanation, movement explanation
		PlayIslandLectureWelcomeGameAndMovementExplanation ();
		yield return new WaitForSeconds(tutorialLectureWelcomeGameAndMovementExplanation.clip.length);
		GameObject movementTutorialDestination = GameObject.Find ("MovementTutorialDestination"); //Enable the orange platform
		movementTutorialDestination.tag = "MovementTutorialDestination";



		StartCoroutine (CheckIfMovementTutorialCompleted(TIME_TO_COMPLETE_MOVEMENT_TUTORIAL));

		yield return null;
	}
		
	void Update() {

		//Check what the user's standing on
		RaycastHit standingOn;

		Physics.Raycast(player.transform.position, Vector3.down, out standingOn);
		//Debug.Log (standingOn.collider.gameObject.name);

		try {
			if (standingOn.collider.gameObject.name == "MovementTutorialDestination" && !movementTutorialCompleted) {
				Debug.Log ("You completed the movement tutorial");
				StartCoroutine (MovementTutorialCompleted ());

			}

			if (standingOn.collider.gameObject.name == "InteractionTutorialDestination" && !interactionTutorialBegun) {
				Debug.Log ("Beginning Interaction Tutorial");
				interactionTutorialBegun = true;
			}

			if (standingOn.collider.gameObject.name == "LaserPointerTutorialDestination" && !laserPointerTutorialBegun) {
				Debug.Log ("Beginning Laser Pointer Tutorial");
				laserPointerTutorialBegun = true;
				StartCoroutine (BeginLaserPointerTutorial ());
			}
		} catch (NullReferenceException e) {
			//Debug.Log ("Not standing on anything");
		}
	} 

	//Disable Teleporting
	//Play a lecture explaining that your controller also works as a laser pointer
	//Enable laser pointer
	//Enable targets
	public IEnumerator BeginLaserPointerTutorial() {
		DisableMovement ();

		EnableLaserPointer ();

		tutorialLectureLaserPointerExplanation.Play ();
		yield return new WaitForSeconds(tutorialLectureLaserPointerExplanation.clip.length);

		targets.SetActive (true);

		yield return null;
	}

	public void CheckIfAllTargetsFound() {
		numberOfTargetsFound++;
		if (numberOfTargetsFound == NUMBER_OF_TARGETS) {
			Debug.Log ("All targets found");
			DisableLaserPointer ();
			StartCoroutine (PortalTutorial ());
		}
	}

	//Runs after all the targets have been found
	//Displays portals
	//Explains how portals work 
	//and then enables movement
	IEnumerator PortalTutorial() {
		UpdateProgress (3); UpdateScore (30);
		NextLevel ();
		tutorialLecturePortalExplanation.Play ();
		yield return new WaitForSeconds(tutorialLecturePortalExplanation.clip.length);

		EnableMovement ();
		yield return null;
	}

	//Runs when the user has clicked the balloon
	public IEnumerator InteractionTutorialCompleted() {
		UpdateProgress (2); UpdateScore (20);
		DisableMovement ();
		interactionTutorialCompleted = true;
		Debug.Log ("Interaction Tutorial Complete");

		tutorialLectureInteractionTutorialComplete.Play ();
		yield return new WaitForSeconds(tutorialLectureInteractionTutorialComplete.clip.length);

		//Wait for lecture to finish before enabling laser pointer destination
		EnableLaserPointerTutorialDestination();
		EnableMovement ();

		yield return null;
	}

	//Runs when it is detected the user is standing on MovementTutorialDestination
	IEnumerator MovementTutorialCompleted() {
		UpdateProgress (5); UpdateScore (50);
		Debug.Log ("Movement tutorial completed");
		movementTutorialCompleted = true;

		DisableMovement ();

		tutorialLectureMovementTutorialComplete.Play ();
		yield return new WaitForSeconds(tutorialLectureMovementTutorialComplete.clip.length);

		//Wait for lecture to finish before enabling the next destination
		EnableInteractionTutorialDestination ();

		EnableMovement ();

		StartCoroutine (CheckIfButtonPressed(TIME_TO_COMPLETE_INTERACTION_TUTORIAL));

		yield return null;
	}

	//Lecture methods
	void PlayIslandLectureWelcomeGameAndMovementExplanation() {
		tutorialLectureWelcomeGameAndMovementExplanation.Play ();
	}

	void EnableInteractionTutorialDestination() {
		GameObject interactionTutorialDestination = GameObject.Find ("InteractionTutorialDestination");
		interactionTutorialDestination.tag = "InteractionTutorialDestination";
	}

	void EnableLaserPointerTutorialDestination() {
		GameObject laserPointerTutorialDestination = GameObject.Find ("LaserPointerTutorialDestination");
		laserPointerTutorialDestination.tag = "LaserPointerTutorialDestination";
	}

	//If 30 seconds have passed and the movement tutorial isn't complete, remind them what to do
	IEnumerator CheckIfMovementTutorialCompleted(int delay) {
		yield return new WaitForSeconds (delay);

		if (!movementTutorialCompleted) {
			UpdateScore (-50);
			Debug.Log ("Playing movement tutorial help");
			DisableMovement ();
			tutorialLectureMovementTutorialHelp.Play();
			yield return new WaitForSeconds(tutorialLectureMovementTutorialHelp.clip.length);
			EnableMovement ();
		}
		yield return null;
	}

	//If 30 seconds have passed and the button isn't pressed, remind them what to do
	IEnumerator CheckIfButtonPressed(int delay) {
		yield return new WaitForSeconds (delay);

		if (!tutorialButtonPressed) {
			UpdateScore (-50);
			Debug.Log ("Playing interaction tutorial help");
			DisableMovement ();
			//Hide the controllers
			GameObject actualLeftController = GameObject.Find("Controller (left)"); GameObject actualRightController = GameObject.Find("Controller (right)");
			actualLeftController.SetActive (false); actualRightController.SetActive (false);
			tutorialLectureInteractionTutorialHelp.Play();
			yield return new WaitForSeconds(tutorialLectureInteractionTutorialHelp.clip.length);
			actualLeftController.SetActive (true); actualRightController.SetActive (true);
			EnableMovement ();
		}
		yield return null;
	}
		
	//Update progress bar model and playerprefs value
	public void UpdateProgress(int progressAmount) {
		ding.Play();
		PlayerPrefs.SetInt ("progress", PlayerPrefs.GetInt ("progress") + progressAmount);
		progressBar.value = PlayerPrefs.GetInt("progress");
	}

	//Overloaded update progress method, used if only updating the model is desired
	public void UpdateProgress(){
		progressBar.value = PlayerPrefs.GetInt("progress");
	}

	//Update secret score
	public void UpdateScore(int scoreAmount) {
		PlayerPrefs.SetInt ("score", PlayerPrefs.GetInt ("score") + scoreAmount);
	}
		
	public void DisableMovement() {
		leftController.enabled = false;
		rightController.enabled = false;
	}

	public void EnableMovement() {
		leftController.enabled = true;
		rightController.enabled = true;
	}

	public void DisableLaserPointer() {
		rightController.GetComponent<LaserPointer> ().enabled = false;
		rightController.GetComponent<VRTK_StraightPointerRenderer> ().enabled = false;
	}

	public void EnableLaserPointer() {
		rightController.GetComponent<LaserPointer> ().enabled = true;
		rightController.GetComponent<VRTK_StraightPointerRenderer> ().enabled = true;
	}


	//Teleport user in front of portals and make them appear 
	public void NextLevel() {

		pm.UpdatePortalStates ();


		GameObject levelPortals = GameObject.Find ("LevelPortals");
		levelPortals.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

		GameObject portals = GameObject.Find ("Portals");
		portals.transform.position = GameObject.Find("LocationForPortalsToAppear").transform.position;
	
	}

}
