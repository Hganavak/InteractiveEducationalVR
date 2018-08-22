using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;

public class SceneManagerPlaygroundHMD : MonoBehaviour {

	public VRTK_Pointer leftController;
	public VRTK_Pointer rightController;

	public Slider progressBar;

	public PortalMaster pm;

	public VRTK_HeightAdjustTeleport teleporter;

	//Game variables
	private const int TIME_TO_EXPLORE = 120; //2 Minutes

	//Lectures
	public AudioSource playgroundLectureIntroductionAndTaskDescription;

	public AudioSource ding;

	// Use this for initialization
	IEnumerator Start () {
		PlayerPrefs.SetInt ("playgroundVisited", 1);

		//Set initial progress bar
		UpdateProgress ();

		PlayPlaygroundLectureIntroductionAndTaskDescription ();
		yield return new WaitForSeconds(playgroundLectureIntroductionAndTaskDescription.clip.length);

		EnableMovement ();
		Invoke("NextLevel", TIME_TO_EXPLORE);
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
	void PlayPlaygroundLectureIntroductionAndTaskDescription() {
		Debug.Log ("Location Information and Task Description Starting....");
		playgroundLectureIntroductionAndTaskDescription.Play ();
	}

	//Teleport user in front of portals and make them appear 
	void NextLevel() {
		UpdateProgress(20); UpdateScore(100);

		pm.UpdatePortalStates ();

		teleporter.ForceTeleport (GameObject.Find("In Front of Portals").transform.position);
	//	GameObject portals = GameObject.Find ("LevelPortals");
	//	portals.transform.position = GameObject.Find("Location for Portals to Appear").transform.position;
	//	portals.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
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
