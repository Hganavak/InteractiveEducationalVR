using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerRoomPC : MonoBehaviour {

	/** Standard Variables **/
	public GameObject player;
	public Slider progressBar;

	public PortalMasterPC pm;

	public AudioSource ding;

	//Shooting
	public int interactionDistance;
	private RaycastHit objectHit;
	//**********************/

	//Game variables
	private const int TIME_TO_FIND_FAULTS = 120; //2 minutes

	public bool doorFound = false;
	public bool foundationIssueFound = false;
	public bool circuitBoxFound = false;
	public bool allFaultsFound = false;

	public GameObject quizOptions;

	//Lectures
	public AudioSource roomLectureLocationInformation;
	public AudioSource roomLectureReconstructionIntroduction;
	public AudioSource roomLectureIssuesNotFound;
	public AudioSource roomLectureQuizIntroduction;
	public AudioSource roomLectureCircuitBoxFound;
	public AudioSource roomLectureDoorFound;
	public AudioSource roomLectureFoundationIssueFound;
	public AudioSource roomLectureIncorrectQuizSelection;
	public AudioSource roomLectureCorrectQuizSelection;

	// Use this for initialization
	IEnumerator Start () {
		PlayerPrefs.SetInt ("roomVisited", 1);
		Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
		UpdateProgress ();
		DisableMovement ();

		//Lecture: Location information and building history
		PlayRoomLectureLocationInformation ();
		yield return new WaitForSeconds(roomLectureLocationInformation.clip.length);

		//Lecture: 3 Reconstructions and task description
		PlayRoomLectureReconstructionIntroduction ();
		yield return new WaitForSeconds(roomLectureReconstructionIntroduction.clip.length);

		//Interaction Task: Find 3 faults
		EnableMovement ();
		StartCoroutine (checkIfSuccessfulAtFindingFaults(TIME_TO_FIND_FAULTS));


		yield return null;
	}

	void Update() {
		if (Input.GetMouseButton (0)) {
			Debug.DrawRay (player.transform.position, player.transform.forward * interactionDistance, Color.blue);

			if (Physics.Raycast (player.transform.position, player.transform.forward, out objectHit, interactionDistance)) {

				//3 Issues
				if (objectHit.transform.name == "CircuitBreakerHalo" && !circuitBoxFound) {
					StartCoroutine ("CircuitBoxFound");
				} 
				if (objectHit.transform.name == "DoorHalo" && !doorFound) {
					StartCoroutine ("DoorFound");
				} 
				if (objectHit.transform.tag== "FoundationIssue" && !foundationIssueFound) {
					Debug.Log (objectHit.transform.name);
					StartCoroutine ("FoundationIssueFound");
				}

				//Quiz
				if (objectHit.transform.tag== "IncorrectQuizSelection") {
					UpdateScore(-25);
					Debug.Log ("Incorrect!");
					objectHit.transform.gameObject.GetComponent<Renderer> ().material.color = Color.red;
					StartCoroutine ("playRoomLectureIncorrectQuizSelection");
				}
				if (objectHit.transform.tag== "CorrectQuizSelection") {
					UpdateScore(25);
					Debug.Log ("Correct!");
					objectHit.transform.gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
					StartCoroutine ("playRoomLectureCorrectQuizSelection");
				}

			}
		}
	}

	IEnumerator checkIfSuccessfulAtFindingFaults(float delay) {
		yield return new WaitForSeconds (delay);

		if (!allFaultsFound) {
			UpdateScore(-100);
			Debug.Log ("You didn't find the faults in time!");
			//roomLectureIssuesNotFound.Play (); Can play over top of other stuff
			highlightFaults ();
		}
	}

	//Highlight all the faults if they haven't been found in time
	void highlightFaults() {
		GameObject[] issueHalos = GameObject.FindGameObjectsWithTag ("IssueHalo");
		foreach (GameObject issueHalo in issueHalos) {
			Behaviour gameObjectHalo = (Behaviour)issueHalo.GetComponent("Halo");
			gameObjectHalo.enabled = true;
		}
	}

	IEnumerator playRoomLectureCorrectQuizSelection() {
		int tmpInteractionDistance = interactionDistance; interactionDistance = 0;
		roomLectureCorrectQuizSelection.Play(); //Lecture that explains the selection was correct
		yield return new WaitForSeconds(roomLectureCorrectQuizSelection.clip.length); // Wait until clip has finished
		pm.EnablePortals();
		interactionDistance = tmpInteractionDistance;
		EnableMovement ();
	}

	IEnumerator playRoomLectureIncorrectQuizSelection() {
		int tmpInteractionDistance = interactionDistance; interactionDistance = 0;
		roomLectureIncorrectQuizSelection.Play(); //Lecture that explains the selection was incorrect
		yield return new WaitForSeconds(roomLectureIncorrectQuizSelection.clip.length); // Wait until clip has finished
		interactionDistance = tmpInteractionDistance;
	}


	IEnumerator CircuitBoxFound() {
		int tmpInteractionDistance = interactionDistance; interactionDistance = 0;
		Debug.Log ("Found the circuit box!");
		DisableMovement ();
		circuitBoxFound = true;
		roomLectureCircuitBoxFound.Play ();
		yield return new WaitForSeconds(roomLectureCircuitBoxFound.clip.length);
		checkIfAllIssuesFound ();
		interactionDistance = tmpInteractionDistance;
	}

	IEnumerator DoorFound() {
		int tmpInteractionDistance = interactionDistance; interactionDistance = 0;
		Debug.Log ("Found the door!");
		DisableMovement ();
		doorFound = true;
		roomLectureDoorFound.Play ();
		yield return new WaitForSeconds(roomLectureDoorFound.clip.length);
		checkIfAllIssuesFound ();
		interactionDistance = tmpInteractionDistance;
	}

	IEnumerator FoundationIssueFound() {
		int tmpInteractionDistance = interactionDistance; interactionDistance = 0;
		Debug.Log ("Found a foundation issue!");
		DisableMovement ();
		foundationIssueFound = true;
		roomLectureFoundationIssueFound.Play ();
		yield return new WaitForSeconds(roomLectureFoundationIssueFound.clip.length);
		checkIfAllIssuesFound ();
		interactionDistance = tmpInteractionDistance;
	}

	public void checkIfAllIssuesFound() {
		if (doorFound && circuitBoxFound && foundationIssueFound) {
			UpdateProgress(10); UpdateScore(100);
			allFaultsFound = true;
			Debug.Log ("You found all the faults");
			StartCoroutine ("doQuiz");
		} else {
			EnableMovement (); //Let them keep looking
		}
	}

	IEnumerator doQuiz() {
		DisableMovement ();
		roomLectureQuizIntroduction.Play ();
		yield return new WaitForSeconds(roomLectureQuizIntroduction.clip.length); //Wait for intro
		quizOptions.SetActive(true);
		EnableMovement ();
		yield return null;
	}

	/********** LECTURE METHODS **********/

	//Lecture: William Woodward Building information and past uses
	void PlayRoomLectureLocationInformation() {
		Debug.Log ("Room Location Information Lecture Starting....");
		roomLectureLocationInformation.Play ();
	}

	//Lecture: 3 Reconstructions and task description
	void PlayRoomLectureReconstructionIntroduction() {
		Debug.Log ("Room Reconstructions and Task Description Lecture Starting....");
		roomLectureReconstructionIntroduction.Play ();
	}
		
	/******** END LECTURE METHODS *******/

	//** STANDARD METHODS **/
	void EnableMovement() {
		GameObject.Find ("FPSController").GetComponent<CharacterController>().enabled = true;
	}

	void DisableMovement() {
		GameObject.Find ("FPSController").GetComponent<CharacterController>().enabled = false;
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

	//*******************************/
}
