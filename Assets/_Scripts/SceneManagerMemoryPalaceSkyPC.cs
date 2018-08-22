using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerMemoryPalaceSkyPC : MonoBehaviour {

	/** Standard Variables **/
	public GameObject player;
	public Slider progressBar;

	public AudioSource ding;

	//Shooting
	public int interactionDistance;
	private RaycastHit objectHit;
	//**********************/

	//Game variables
	private const int TIME_TO_EXPLORE = 300; //5 Minutes
	public AudioSource skyMemoryPalaceIntroduction;

	public bool northWestActivatorUsed;
	public bool northEastActivatorUsed;
	public bool southWestActivatorUsed;
	public bool southEastActivatorUsed;

	public GameObject gameOverRoom; //To enable/disable game over room
	private bool gameOver;

	public TextMesh scoreText;

	IEnumerator Start() {
		Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;

		PlayerPrefs.SetInt ("progress", 80);
		UpdateProgress();

		DisableMovement ();

		skyMemoryPalaceIntroduction.Play ();
		yield return new WaitForSeconds(skyMemoryPalaceIntroduction.clip.length);

		EnableMovement ();
		//Invoke ("GameOver", TIME_TO_EXPLORE);

		yield return null;
	}

	void Update() {
		if (Input.GetMouseButton (0)) {
			Debug.DrawRay (player.transform.position, player.transform.forward * interactionDistance, Color.blue);

			if (Physics.Raycast (player.transform.position, player.transform.forward, out objectHit, interactionDistance)) {

				//South East Corner
				if (objectHit.transform.name == "SouthEastActivatorButton" && !southEastActivatorUsed) {
					southEastActivatorUsed = true;
					objectHit.transform.gameObject.GetComponent<Renderer> ().material.color = Color.red;
					GameObject.Find ("SouthEastCornerPointLight").SetActive (false);
					UpdateScore(50); UpdateProgress(5);
				} 

				//South West Corner
				if (objectHit.transform.name == "SouthWestActivatorButton" && !southWestActivatorUsed) {
					southWestActivatorUsed = true;
					objectHit.transform.gameObject.GetComponent<Renderer> ().material.color = Color.red;
					GameObject.Find ("SouthWestCornerPointLight").SetActive (false);
					UpdateScore(50); UpdateProgress(5);
				}

				//North West Corner
				if (objectHit.transform.name == "NorthWestActivatorButton" && !northWestActivatorUsed) {
					northWestActivatorUsed = true;
					objectHit.transform.gameObject.GetComponent<Renderer> ().material.color = Color.red;
					GameObject.Find ("NorthWestCornerPointLight").SetActive (false);
					UpdateScore(50); UpdateProgress(5);
				}

				//North East Corner
				if (objectHit.transform.name == "NorthEastActivatorButton" && !northEastActivatorUsed) {
					northEastActivatorUsed = true;
					objectHit.transform.gameObject.GetComponent<Renderer> ().material.color = Color.red;
					GameObject.Find ("NorthEastCornerPointLight").SetActive (false);
					UpdateScore(50); UpdateProgress(5);
				}

				CheckIfGameOver ();
			}
		}
	}

	public void CheckIfGameOver() {
		if (northWestActivatorUsed && northEastActivatorUsed && southWestActivatorUsed && southEastActivatorUsed) {
			UpdateScore (5);
			GameOver ();
		}
	}

	public void GameOver() {
		if (!gameOver) { //Stops method running twice if start invokes it after time limit
			gameOver = true;
			Debug.Log ("Game Over");
			gameOverRoom.SetActive (true);
			GameObject.Find("FPSController").transform.position = GameObject.Find("In Game Over Room").transform.position;

			scoreText.text = (string)PlayerPrefs.GetInt ("score").ToString ();

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
