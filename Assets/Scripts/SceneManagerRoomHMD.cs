using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;

public class SceneManagerRoomHMD : MonoBehaviour {

	public VRTK_Pointer leftController;
	public VRTK_Pointer rightController;

	public Slider progressBar;

	public PortalMaster pm;

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

	public AudioSource ding;

	// Use this for initialization
	IEnumerator Start () {
		
		PlayerPrefs.SetInt ("roomVisited", 1);

		//Set initial progress bar
		UpdateProgress ();

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

	IEnumerator doQuiz() {
		DisableMovement ();
		roomLectureQuizIntroduction.Play ();
		yield return new WaitForSeconds(roomLectureQuizIntroduction.clip.length); //Wait for intro
		quizOptions.SetActive(true);
		EnableMovement ();
		yield return null;
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

	public void NextLevel() {
		pm.UpdatePortalStates ();

		GameObject portals = GameObject.Find ("LevelPortals");
		portals.transform.position = GameObject.Find("Location for Portals to Appear").transform.position;
		portals.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
	}

}
