using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SceneManagerMountainHMD : SceneManagerHMD {

	//Lectures
	public AudioSource MountainLectureIntroductionAndTaskDescription;
	public AudioSource MountainLectureAllRubbleFoundAndLaserPointerTaskDescription;
	public AudioSource MountainLectureNextLevel;

	//Game variables
	private int piecesOfRubbleFound;
	private bool allRubbleCollected;
	private bool mountainHeightMeasured;
	private const float TIME_TO_MEAUSURE_MOUNTAIN = 15f; //15 seconds

	public GameObject mountainCollider;
	public GameObject heightText;

	// Use this for initialization
	IEnumerator Start () {

		PlayerPrefs.SetInt ("mountainVisited", 1);
	

		//Set initial progress bar
		UpdateProgress ();

		DisableMovement ();

		//Introduction lecture
		MountainLectureIntroductionAndTaskDescription.Play();
		yield return new WaitForSeconds(MountainLectureIntroductionAndTaskDescription.clip.length);

		//Turn off the 2 volcano halos
		GameObject.Find ("LeftVolcanoHalo").SetActive (false);
		GameObject.Find ("LeftmostVolcanoHalo").SetActive (false);


		EnableMovement ();


		yield return null;
	}

	public void rubbleFound() {
		if (!allRubbleCollected) {
			piecesOfRubbleFound++;
			UpdateProgress (3);
			UpdateScore (30);

			if (piecesOfRubbleFound >= 3) {
				allRubbleCollected = true;
				Debug.Log ("You dun it");
				StartCoroutine (PlayMountainLectureAllRubbleFoundAndLaserPointerTaskDescription ());
			}
		}
	}

	private IEnumerator PlayMountainLectureAllRubbleFoundAndLaserPointerTaskDescription() {
		DisableMovement ();

		MountainLectureAllRubbleFoundAndLaserPointerTaskDescription.Play();
		yield return new WaitForSeconds(MountainLectureAllRubbleFoundAndLaserPointerTaskDescription.clip.length);

		Invoke ("EnableMountainColliderHalo", TIME_TO_MEAUSURE_MOUNTAIN);
		EnableLaserPointer ();
		yield return null;
	}

	void EnableMountainColliderHalo() {
		if (!mountainHeightMeasured) {
			Behaviour gameObjectHalo = (Behaviour)mountainCollider.GetComponent ("Halo");
			gameObjectHalo.enabled = true;
		}
	}

	public void DisableLaserPointer() {
		rightController.GetComponent<LaserPointer> ().enabled = false;
		rightController.GetComponent<VRTK_StraightPointerRenderer> ().enabled = false;
	}

	public void EnableLaserPointer() {
		rightController.GetComponent<LaserPointer> ().enabled = true;
		rightController.GetComponent<VRTK_StraightPointerRenderer> ().enabled = true;
	}

	public void MountainMeasured() {
		mountainHeightMeasured = true;
		Debug.Log ("Measured the mountain");
		UpdateProgress (11); UpdateScore (110);
		heightText.SetActive (true);
		DisableLaserPointer ();

		StartCoroutine (PlayMountainLectureNextLevel ());
	}

	private IEnumerator PlayMountainLectureNextLevel() {
		NextLevel ();

		MountainLectureNextLevel.Play ();
		yield return new WaitForSeconds (MountainLectureNextLevel.clip.length);


		EnableMovement ();
		yield return null;
	}

	public void NextLevel() {
		pm.UpdatePortalStates ();

		GameObject portals = GameObject.Find ("LevelPortals");
		portals.transform.position = GameObject.Find("Location for Portals to Appear").transform.position;
		portals.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
	}

}
