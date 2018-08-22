using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;

public class SceneManagerMemoryPalaceSkyHMD : MonoBehaviour {

	public VRTK_Pointer leftController;
	public VRTK_Pointer rightController;

	public Slider progressBar;

	//Game variables
	public bool northWestActivatorUsed;
	public bool northEastActivatorUsed;
	public bool southWestActivatorUsed;
	public bool southEastActivatorUsed;

	private const int TIME_TO_EXPLORE = 300; //5 Minutes

	public VRTK_HeightAdjustTeleport teleporter;

	public GameObject gameOverRoom; //To enable/disable game over room
	public TextMesh scoreText;

	public AudioSource skyMemoryPalaceIntroduction;

	public AudioSource ding;

	private bool gameOver;

	IEnumerator Start() {
		
		PlayerPrefs.SetInt ("progress", 80);
		UpdateProgress();

		skyMemoryPalaceIntroduction.Play ();
		yield return new WaitForSeconds(skyMemoryPalaceIntroduction.clip.length);

		EnableMovement ();
		Invoke ("GameOver", TIME_TO_EXPLORE);
		PlayerPrefs.SetInt ("score", 69);

		yield return null;
	}

	public void CheckIfGameOver() {
		if (northWestActivatorUsed && northEastActivatorUsed && southWestActivatorUsed && southEastActivatorUsed) {
			PlayerPrefs.SetInt ("score", PlayerPrefs.GetInt("score")+5);
			GameOver ();
		}
	}

	public void GameOver() {
		if (!gameOver) { //Stops method running twice if start invokes it after time limit
			gameOver = true;
			Debug.Log ("Game Over");
			gameOverRoom.SetActive (true);
			teleporter.ForceTeleport (GameObject.Find("In Game Over Room").transform.position);

			scoreText.text = (string)PlayerPrefs.GetInt ("score").ToString ();

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
}
