using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerIslandPC : MonoBehaviour {

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
	public AudioSource islandLectureShellfishFound;
	public AudioSource islandLectureShellfishNotFound;
	public AudioSource islandLecturePlantsIntroduction;
	public AudioSource islandLectureBananaTreeFound;
	public AudioSource islandLectureHeliconiaTreeFound;
	public AudioSource islandLectureArrowheadPlantFound;
	public AudioSource islandLectureAllPlantsFound;




	// Use this for initialization
	IEnumerator Start () {
		PlayerPrefs.SetInt ("islandVisited", 1);

		Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
		UpdateProgress ();

		DisableMovement ();

		//Lecture: Location information and Trobriand Cricket
		PlayIslandLectureLocationInformation ();
		yield return new WaitForSeconds(islandLectureLocationInformation.clip.length);

		//Lecture: Shellfish introduction and task description 
		PlayIslandLectureShellfishIntroduction ();
		yield return new WaitForSeconds(islandLectureShellfishIntroduction.clip.length);

		EnableMovement ();

		//Interaction Task: Grey Shellfish
		EnableMovement ();
		StartCoroutine(checkIfSuccessfulAtFindingShell(TIME_TO_FIND_SHELLFISH)); //Check if user has found the shell after specified time seconds

		yield return null;
	}

	//Shooting checks
	void Update() {
		checkIfLost ();
		if (Input.GetMouseButton (0)) {
			Debug.DrawRay (player.transform.position, player.transform.forward * interactionDistance, Color.blue);

			if (Physics.Raycast (player.transform.position, player.transform.forward, out objectHit, interactionDistance)) {
				//Do all the checks
				if (objectHit.transform.name == "grey_shellfish" && !shellfishFound) {
					Debug.Log ("Found the shellfish!");
					shellfishFound = true;
					StartCoroutine ("playIslandLectureShellfishFound");
				}
				//Trees/Plants
				if (objectHit.transform.tag == "BananaTree" && !bananaTreeFound) {
					Debug.Log ("Found a banana tree!");
					bananaTreeFound = true;
					StartCoroutine ("PlayIslandLectureBananaTreeFound");
				}
				if (objectHit.transform.tag == "HeliconiaTree" && !heliconiaTreeFound) {
					Debug.Log ("Found a heliconia tree!");
					heliconiaTreeFound = true;
					StartCoroutine ("PlayIslandLectureHeliconiaTreeFound");
				}
				if (objectHit.transform.tag == "ArrowheadPlant" && !arrowheadPlantFound) {
					Debug.Log ("Found an arrowhead plant!");
					arrowheadPlantFound = true;
					StartCoroutine ("PlayIslandLectureArrowheadPlantFound");
				}
			}
		}
	}

	void checkIfLost() {
		float distanceFromCenterX = Mathf.Abs( player.transform.position.x - GameObject.Find ("Center").transform.position.x);
		float distanceFromCenterZ = Mathf.Abs( player.transform.position.z - GameObject.Find ("Center").transform.position.z);
		if (distanceFromCenterX > 125 || distanceFromCenterZ > 115) {
			Debug.Log ("Lost (x, z): (" + distanceFromCenterX + ", "+ distanceFromCenterZ + ")");
			GameObject.Find("FPSController").transform.position = GameObject.Find("Starting Position").transform.position;
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

	//When user has found all the plants
	IEnumerator PlayIslandLectureAllPlantsFound(){
		DisableMovement ();
		pm.EnablePortals();
		islandLectureAllPlantsFound.Play ();
		Debug.Log ("All plants found lecture starting...");
		GameObject.Find("FPSController").transform.position = GameObject.Find("In Front of Portals").transform.position;
		yield return new WaitForSeconds(islandLectureAllPlantsFound.clip.length); // Wait until clip has finished before doing portal stuff
		EnableMovement ();
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

	IEnumerator playIslandLectureShellfishFound() {
		DisableMovement ();
		UpdateProgress (5); UpdateScore (50);
		islandLectureShellfishFound.Play (); //Play a short clip saying that the object has been found

		yield return new WaitForSeconds (islandLectureShellfishFound.clip.length); //Wait until lecture's finished before disabling the shell, and enabling movement
		Destroy(greyShellfish.GetComponent<Pickupable> ()); //Force user to drop the shellfish and then make it so it can't be picked up
		greyShellfish.GetComponent<HighlightShellfish>().isHighlightable = false;
		StartCoroutine ("PlayIslandLecturePlantsIntroduction");
	}

	//Lecture: Plants introduction and task description found
	public IEnumerator PlayIslandLecturePlantsIntroduction() {
		Debug.Log ("Plants Introduction Lecture Starting....");
		islandLecturePlantsIntroduction.Play ();
		yield return new WaitForSeconds(islandLecturePlantsIntroduction.clip.length); // Wait until clip has finished before enabling movement
		Invoke("highlightPlants", TIME_TO_FIND_PLANTS); //Highlight the plants if they haven't been found in 3 minutes
		EnableMovement();
	}

	//Lecture: Banana Tree Found
	IEnumerator PlayIslandLectureBananaTreeFound() {
		Debug.Log ("Banana Tree Found Lecture Starting....");
		islandLectureBananaTreeFound.Play ();
		yield return new WaitForSeconds(islandLectureBananaTreeFound.clip.length);
		checkIfAllPlantsFound ();
	}

	//Lecture: Heliconia Tree Found
	IEnumerator PlayIslandLectureHeliconiaTreeFound() {
		Debug.Log ("Heliconia Tree Found Lecture Starting....");
		islandLectureHeliconiaTreeFound.Play ();
		yield return new WaitForSeconds(islandLectureHeliconiaTreeFound.clip.length);
		checkIfAllPlantsFound ();
	}

	//Lecture: Arrowhead Plant Found
	IEnumerator PlayIslandLectureArrowheadPlantFound() {
		Debug.Log ("Arrowhead Plant Found Lecture Starting....");
		islandLectureArrowheadPlantFound.Play ();
		yield return new WaitForSeconds(islandLectureArrowheadPlantFound.clip.length);
		checkIfAllPlantsFound ();
	}


	/************************************/

	/*	if 60 seconds have passed and the shellfish hasn't been found
	*  Teleport user to shell, highlight it, and then tell the user where it is
	* 
	*/
	IEnumerator checkIfSuccessfulAtFindingShell(float delay) {

		yield return new WaitForSeconds (delay); //Wait this long before running the code below

		if (!shellfishFound) {
			UpdateScore (-110);
			DisableMovement ();
			shellfishFound = true;
			Debug.Log("Shellfish not found lecture starting...!");
			GameObject.Find("FPSController").transform.position = GameObject.Find("In Front of Shellfish").transform.position;
			StartCoroutine (ToggleHalo(greyShellfish, 6));
			islandLectureShellfishNotFound.Play ();
			yield return new WaitForSeconds(islandLectureShellfishNotFound.clip.length+1f); // Wait until clip has finished before starting the plants interaction task
			Destroy(greyShellfish.GetComponent<Pickupable> ()); //Force user to drop the shellfish and then make it so it can't be picked up
			greyShellfish.GetComponent<HighlightShellfish>().isHighlightable = false;
			StartCoroutine ("PlayIslandLecturePlantsIntroduction");
		} 
	}

	//Toggles a Halo component on and off a GameObject after a specified time
	IEnumerator ToggleHalo(GameObject haloGameObject, int delayTime) {
		Behaviour gameObjectHalo = (Behaviour)haloGameObject.GetComponent("Halo");
		gameObjectHalo.enabled = true;

		yield return new WaitForSeconds (delayTime);
		gameObjectHalo.enabled = false;
	}

	//Highlight plants if they haven't been found in 2 minutes
	void highlightPlants() {
		UpdateScore (-110);
		Debug.Log ("Highlighting plants!");
		string[] plantTags = {
			"BananaTree",
			"HeliconiaTree",
			"ArrowheadPlant"
		};
		foreach (string plantTag in plantTags) {
			GameObject[] plants = GameObject.FindGameObjectsWithTag (plantTag);
			Debug.Log (plants.Length);
			foreach (GameObject plant in plants) {
				Behaviour gameObjectHalo = (Behaviour)plant.GetComponent("Halo");
				gameObjectHalo.enabled = true;
			}
		}
	}

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