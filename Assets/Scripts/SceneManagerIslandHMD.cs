using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;


public class SceneManagerIslandHMD : MonoBehaviour {

	public GameObject player;

	public VRTK_Pointer leftController;
	public VRTK_Pointer rightController;

	public Slider progressBar;

	public VRTK_HeightAdjustTeleport teleporter;

	public PortalMaster pm;

	public Transform startingPlayerPosition;

	//Game variables
	private const int TIME_TO_FIND_PLANTS = 180; //2 Minutes
	private const int TIME_TO_FIND_SHELLFISH = 60; //1 minutes

	//Interction task: Find shellfish
	public GameObject greyShellfish;
	public static bool shellfishFound;

	//Interaction task: Identify 3 species of plants
	public static bool bananaTreeFound;
	public static bool heliconiaTreeFound;
	public static bool arrowheadPlantFound;

	//Lectures
	public AudioSource islandLectureLocationInformation;
	public AudioSource islandLectureShellfishIntroduction;
	public AudioSource islandLectureShellfishNotFound;
	public AudioSource islandLecturePlantsIntroduction;
	public AudioSource islandLectureAllPlantsFound;

	public AudioSource ding;

	// Use this for initialization
	IEnumerator Start () {

		PlayerPrefs.SetInt ("islandVisited", 1);

		//Set initial progress bar
		UpdateProgress ();

		//Lecture: Location information and Trobriand Cricket
		PlayIslandLectureLocationInformation ();

		yield return new WaitForSeconds(islandLectureLocationInformation.clip.length);

		//Lecture: Shellfish introduction and task description 
		PlayIslandLectureShellfishIntroduction ();
		yield return new WaitForSeconds(islandLectureShellfishIntroduction.clip.length);

		//Interaction Task: Grey Shellfish
		EnableMovement ();
		StartCoroutine(checkIfSuccessfulAtFindingShell(TIME_TO_FIND_SHELLFISH)); //Check if user has found the shell after specified time seconds

	}

	void Update() {
		float distanceFromCenterX = Mathf.Abs( player.transform.position.x - GameObject.Find ("Center").transform.position.x);
		float distanceFromCenterZ = Mathf.Abs( player.transform.position.z - GameObject.Find ("Center").transform.position.z);
		if (distanceFromCenterX > 125 || distanceFromCenterZ > 115) {
			Debug.Log ("Lost (x, z): (" + distanceFromCenterX + ", "+ distanceFromCenterZ + ")");
			teleporter.ForceTeleport (GameObject.Find("Starting Position").transform.position);
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

	/********** LECTURE METHODS **********/

	//Lecture: Island Introduction and Trobriand Cricket
	void PlayIslandLectureLocationInformation() {
		Debug.Log ("Location Information Lecture Starting....");
		islandLectureLocationInformation.Play ();
	}

	//Lecture: Shellfish Introduction and task description
	void PlayIslandLectureShellfishIntroduction() {
		Debug.Log ("Shellfish Introduction Lecture Starting....");
		islandLectureShellfishIntroduction.Play ();
	}

	//Lecture: Plants introduction and task description found
	public IEnumerator PlayIslandLecturePlantsIntroduction() {
		Debug.Log ("Plants Introduction Lecture Starting....");
		islandLecturePlantsIntroduction.Play ();
		yield return new WaitForSeconds(islandLecturePlantsIntroduction.clip.length); // Wait until clip has finished before enabling movement
		Invoke("highlightPlants", TIME_TO_FIND_PLANTS); //Highlight the plants if they haven't been found in 3 minutes
		EnableMovement();
	}

	//Lecture: When user has found all the plants
	IEnumerator PlayIslandLectureAllPlantsFound(){
		DisableMovement ();
		islandLectureAllPlantsFound.Play ();
		Debug.Log ("All plants found lecture starting...");
		yield return new WaitForSeconds(islandLectureAllPlantsFound.clip.length); // Wait until clip has finished before doing portal stuff
		NextLevel();
		EnableMovement ();
	}

	//Teleport user in front of portals and make them appear 
	void NextLevel() {

		pm.UpdatePortalStates ();

		teleporter.ForceTeleport (GameObject.Find("In Front of Portals").transform.position);
		GameObject portals = GameObject.Find ("LevelPortals");
		portals.transform.position = GameObject.Find("Location for Portals to Appear").transform.position;
		portals.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
	}


	/******** END LECTURE METHODS *******/

	/*	if 60 seconds have passed and the shellfish hasn't been found
	 *  Teleport user to shell, highlight it, and then tell the user where it is
	 * 
	 */
	IEnumerator checkIfSuccessfulAtFindingShell(float delay) {

		yield return new WaitForSeconds (delay); //Wait this long before running the code below

		if (!SceneManagerIslandHMD.shellfishFound) {
			UpdateScore (-110);
			DisableMovement ();
			greyShellfish.GetComponent<VRTK_InteractableObject>().isUsable = false; //Disable shellfish
			greyShellfish.GetComponent<VRTK_InteractableObject>().isGrabbable = false;
			Debug.Log("Shellfish not found lecture starting...!");
			teleporter.ForceTeleport (GameObject.Find("In Front of Shellfish").transform.position);
			StartCoroutine (ToggleHalo(greyShellfish, 6));
			islandLectureShellfishNotFound.Play ();
			yield return new WaitForSeconds(islandLectureShellfishNotFound.clip.length+1f); // Wait until clip has finished before starting the plants interaction task
			StartCoroutine ("PlayIslandLecturePlantsIntroduction");
		} 
	}

	//Highlight plants if they haven't been found in 2 minutes
	void highlightPlants() {
		UpdateScore (-110);
		Debug.Log ("Highlighting plants!");
		GameObject[] plants = GameObject.FindGameObjectsWithTag ("Plant");
		Debug.Log (plants.Length);
		foreach (GameObject plant in plants) {
			Behaviour gameObjectHalo = (Behaviour)plant.GetComponent("Halo");
			gameObjectHalo.enabled = true;
		}
	}
		

	/**
	 * Called by the plant scripts.
	 * Checks if all 3 plants have been found
	 */
	public void checkIfAllPlantsFound() {
		if (bananaTreeFound && heliconiaTreeFound && arrowheadPlantFound) {
			UpdateProgress (15); UpdateScore(150);
			StartCoroutine(PlayIslandLectureAllPlantsFound ());
		}
	}

	//Toggles a Halo component on and off a GameObject after a specified time
	IEnumerator ToggleHalo(GameObject haloGameObject, int delayTime) {
		Behaviour gameObjectHalo = (Behaviour)haloGameObject.GetComponent("Halo");
		gameObjectHalo.enabled = true;

		yield return new WaitForSeconds (delayTime);
		gameObjectHalo.enabled = false;
	}


	public void DisableMovement() {
		leftController.enabled = false;
		rightController.enabled = false;
	}

	public void EnableMovement() {
		leftController.enabled = true;
		rightController.enabled = true;
	}

}