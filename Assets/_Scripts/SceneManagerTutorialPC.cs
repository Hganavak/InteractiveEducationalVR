using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerTutorialPC : MonoBehaviour {

	public GameObject player;
	public Slider progressBar;

	public PortalMasterPC pm;

	//Shooting
	public int interactionDistance;
	private RaycastHit objectHit;

	//Game variables
	public bool movementTutorialCompleted = false;
	public const int TIME_TO_COMPLETE_MOVEMENT_TUTORIAL = 20; //20 seconds

	public static bool interactionTutorialBegun;
	public bool tutorialButtonPressed;
	public bool interactionTutorialCompleted;
	public const int TIME_TO_COMPLETE_INTERACTION_TUTORIAL = 30; //30 seconds

	public bool ballTutorialCompleted;

	public GameObject cornerLight;
	public GameObject ball;

	public bool laserPointerTutorialDestinationTriggered;
	public GameObject laserGun;

	public GameObject targets;


	//Lectures
	public AudioSource tutorialLectureWelcomeGameAndMovementExplanation;

	public AudioSource tutorialLectureMovementTutorialHelp;
	public AudioSource tutorialLectureMovementTutorialComplete;

	public AudioSource tutorialLectureInteractionTutorialHelp;
	public AudioSource tutorialLectureInteractionTutorialComplete;
	public AudioSource tutorialLectureInteractionTutorialBall;

	public AudioSource tutorialLectureLaserPointerExplanation;

	public AudioSource tutorialLecturePortalExplanation;

	public AudioSource ding;

	public int numberOfTargetsFound;
	public const int NUMBER_OF_TARGETS = 5;

	// Use this for initialization
	IEnumerator Start () {

		Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
		progressBar.value = PlayerPrefs.GetInt ("progress");

		ResetScores ();

		UpdateProgress (5);
		DisableMovement ();


		//Introduction lecture
		tutorialLectureWelcomeGameAndMovementExplanation.Play();
		yield return new WaitForSeconds (tutorialLectureWelcomeGameAndMovementExplanation.clip.length);
		EnableMovement ();

		StartCoroutine (CheckIfMovementTutorialCompleted(TIME_TO_COMPLETE_MOVEMENT_TUTORIAL));

		yield return null;
		//Invoke ("DisableMovement", 5);
		//Invoke ("EnableMovement", 10);
	}

	//Shooting Checks
	void Update()
	{
		if (Input.GetMouseButton (0)) {
			Debug.DrawRay (player.transform.position, player.transform.forward * interactionDistance, Color.blue);

			if (Physics.Raycast (player.transform.position, player.transform.forward, out objectHit, interactionDistance)) {
				//Debug.Log ("You shot: " + objectHit.transform.name);
				//Do all the checks
				if (objectHit.transform.name == "ActivatorButton" && movementTutorialCompleted && !interactionTutorialCompleted && interactionTutorialBegun) {
					Debug.Log ("Activated the button");
					UpdateProgress (5);
					UpdateScore (50);
					tutorialButtonPressed = true;
					interactionTutorialCompleted = true;
					objectHit.transform.gameObject.GetComponent<Highlightable> ().isHighlightable = false; //Disable highlighting
					objectHit.transform.gameObject.GetComponent<Renderer> ().material.color = Color.red;
					StartCoroutine (BallTutorial ());
				} else if (objectHit.transform.name == "Ball" && !ballTutorialCompleted) {
					Debug.Log ("Activated the ball");
					ballTutorialCompleted = true;
					//ball.GetComponent<Highlightable> ().isHighlightable = false;
					//ball.GetComponent<Renderer> ().material.color = Color.yellow;
					StartCoroutine (InteractionTutorialCompleted ());
				}
				else if (objectHit.transform.gameObject.tag == "Target" && laserGun.GetComponent<LaserPointerPC>().LaserActive) {
					ding.Play ();
					Debug.Log ("Activated the ball");
					ballTutorialCompleted = true;
					Destroy(objectHit.transform.gameObject);
					CheckIfAllTargetsFound ();
				}

			} 
		}
	}//End Update()


	//Runs when the user has clicked the balloon
	public IEnumerator InteractionTutorialCompleted() {
		UpdateProgress (2); UpdateScore (20);
		DisableMovement ();
		interactionTutorialCompleted = true;
		Debug.Log ("Interaction Tutorial Complete");

		tutorialLectureInteractionTutorialComplete.Play ();
		yield return new WaitForSeconds(tutorialLectureInteractionTutorialComplete.clip.length);

		//Wait for lecture to finish before enabling laser pointer destination
		laserPointerTutorialDestinationTriggered = true;
		EnableMovement ();

		yield return null;
	}

	//Called from MovementTutorialComplete script attached to orange cube
	public IEnumerator MovementTutorialComplete() {
		Debug.Log ("Move complete");
		if (!movementTutorialCompleted) {
			movementTutorialCompleted = true;
			DisableMovement ();

			UpdateProgress (5); UpdateScore (50);

			tutorialLectureMovementTutorialComplete.Play ();
			yield return new WaitForSeconds(tutorialLectureMovementTutorialComplete.clip.length);

			interactionTutorialBegun = true;
			EnableMovement ();

			StartCoroutine (CheckIfButtonPressed(TIME_TO_COMPLETE_INTERACTION_TUTORIAL));
		}
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

			tutorialLectureInteractionTutorialHelp.Play();
			yield return new WaitForSeconds(tutorialLectureInteractionTutorialHelp.clip.length);

			EnableMovement ();
		}
		yield return null;
	}
		
	IEnumerator BallTutorial() {

		DisableMovement ();
		tutorialLectureInteractionTutorialBall.Play ();
		yield return new WaitForSeconds(tutorialLectureInteractionTutorialBall.clip.length);

		cornerLight.SetActive (true);
		ball.SetActive (true);

		yield return null;
	}

	//Disable Movement
	//Play a lecture explaining that your mouse also works as a laser pointer
	//Enable laser pointer
	//Enable targets
	public IEnumerator BeginLaserPointerTutorial() {
		laserPointerTutorialDestinationTriggered = false;

		DisableMovement ();

		EnableLaserGun ();

		tutorialLectureLaserPointerExplanation.Play ();
		yield return new WaitForSeconds(tutorialLectureLaserPointerExplanation.clip.length);

		targets.SetActive (true);

		yield return null;
	}

	public void CheckIfAllTargetsFound() {
		numberOfTargetsFound++;
		if (numberOfTargetsFound == NUMBER_OF_TARGETS) {
			Debug.Log ("All targets found");
			DisableLaserGun ();
			StartCoroutine (PortalTutorial ());
		}
	}

	//Runs after all the targets have been found
	//Displays portals
	//Explains how portals work 
	//and then enables movement
	IEnumerator PortalTutorial() {
		UpdateProgress (3); UpdateScore (30);
		pm.EnablePortals();
		tutorialLecturePortalExplanation.Play ();
		yield return new WaitForSeconds(tutorialLecturePortalExplanation.clip.length);

		EnableMovement ();
		yield return null;
	}

	void EnableMovement() {
		GameObject.Find ("FPSController").GetComponent<CharacterController>().enabled = true;
	}

	void DisableMovement() {
		GameObject.Find ("FPSController").GetComponent<CharacterController>().enabled = false;
	}

	void EnableLaserGun() {
		interactionDistance = 500;
		laserGun.SetActive (true);
	}

	void DisableLaserGun() {
		interactionDistance = 3;
		laserGun.SetActive (false);
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

	//Replaces Anorak as it wasn't working
	public void ResetScores() {
		PlayerPrefs.DeleteAll ();//RESET PLAYER PREFS
		PlayerPrefs.SetInt ("progress", 0);
		PlayerPrefs.SetInt ("score", 0);
		PlayerPrefs.SetInt ("forestVisited", 0);
		PlayerPrefs.SetInt ("lectureTheatreVisited", 0);
		PlayerPrefs.SetInt ("mountainVisited", 0);
	}
}
