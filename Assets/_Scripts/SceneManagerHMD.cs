using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class SceneManagerHMD : MonoBehaviour {

	public GameObject player;

	public VRTK_Pointer leftController;
	public VRTK_Pointer rightController;
	public Slider progressBar;

	public PortalMaster pm;

	public AudioSource ding;

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

}
