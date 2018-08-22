using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerPlaygroundPC : MonoBehaviour {

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
	private const int TIME_TO_EXPLORE = 120; //2 Minutes

	//Lectures
	public AudioSource playgroundLectureIntroductionAndTaskDescription;

	/******** END LECTURE METHODS *******/

	// Use this for initialization
	IEnumerator Start () {
		Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;

		PlayerPrefs.SetInt ("playgroundVisited", 1);

		//Set initial progress bar
		UpdateProgress ();

		playgroundLectureIntroductionAndTaskDescription.Play ();
		yield return new WaitForSeconds(playgroundLectureIntroductionAndTaskDescription.clip.length);

		EnableMovement ();
		Invoke("NextLevel", TIME_TO_EXPLORE);

		EnableMovement ();
		Invoke("NextLevel", TIME_TO_EXPLORE);

	}

	//Teleport user in front of portals and make them appear 
	void NextLevel() {
		UpdateProgress(20); UpdateScore(100);
		pm.EnablePortals ();
		GameObject.Find("FPSController").transform.position = GameObject.Find("In Front of Portals").transform.position;
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
