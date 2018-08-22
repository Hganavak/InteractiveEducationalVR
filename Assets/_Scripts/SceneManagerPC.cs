using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerPC : MonoBehaviour {

	/** Standard Variables **/
	public GameObject player;
	public Slider progressBar;

	public AudioSource ding;

	//Shooting
	public int interactionDistance;
	private RaycastHit objectHit;
	//**********************/

	//** STANDARD METHODS **/
	public void EnableMovement() {
		GameObject.Find ("FPSController").GetComponent<CharacterController>().enabled = true;
	}

	public void DisableMovement() {
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
