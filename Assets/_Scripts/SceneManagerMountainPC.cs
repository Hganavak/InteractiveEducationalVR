using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerMountainPC : SceneManagerPC {

	public PortalMasterPC pm;

	//Shooting
	private RaycastHit objectHit;
	public GameObject laserGun;

	//Lectures
	public AudioSource MountainLectureIntroductionAndTaskDescription;
	public AudioSource MountainLectureAllRubbleFoundAndLaserPointerTaskDescription;
	public AudioSource MountainLectureNextLevel;

	//Game variables
	private int piecesOfRubbleFound;
	private bool mountainHeightMeasured;
	private const float TIME_TO_MEAUSURE_MOUNTAIN = 15f; //15 seconds

	public GameObject mountainCollider;

	public GameObject heightText;

	// Use this for initialization
	IEnumerator Start () {
		Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;

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

	//Shooting Checks
	void Update()
	{
		if (Input.GetMouseButton (0)) {
			Debug.DrawRay (player.transform.position, player.transform.forward * interactionDistance, Color.blue);

			if (Physics.Raycast (player.transform.position, player.transform.forward, out objectHit, interactionDistance)) {
				//Debug.Log ("You shot: " + objectHit.transform.name);
				//Do all the checks
				if (objectHit.transform.tag == "Rubble" && piecesOfRubbleFound<3) {
					RubbleFound ();
					Destroy(objectHit.transform.gameObject.GetComponent<BoxCollider> ());
				} 
				if (objectHit.transform.name == "Mountain Collider" && !mountainHeightMeasured) {
					mountainHeightMeasured = true;
					Debug.Log ("Measured the mountain");
					UpdateProgress (11); UpdateScore (110);
					heightText.SetActive (true);
					Destroy(objectHit.transform.gameObject.GetComponent<BoxCollider> ());
					StartCoroutine (PlayMountainLectureNextLevel ());
				} 
			} 
		}
	}//End Update()

	private void RubbleFound() {
		Debug.Log ("You found some rubble");
		piecesOfRubbleFound++;
		UpdateProgress (3); UpdateScore (30);

		if (piecesOfRubbleFound >= 3) {
			Debug.Log ("You dun it");
			StartCoroutine (PlayMountainLectureAllRubbleFoundAndLaserPointerTaskDescription ());
		}
	}

	private IEnumerator PlayMountainLectureAllRubbleFoundAndLaserPointerTaskDescription() {
		DisableMovement ();

		MountainLectureAllRubbleFoundAndLaserPointerTaskDescription.Play();
		yield return new WaitForSeconds(MountainLectureAllRubbleFoundAndLaserPointerTaskDescription.clip.length);

		Invoke ("EnableMountainColliderHalo", TIME_TO_MEAUSURE_MOUNTAIN);
		EnableLaserGun ();
		yield return null;
	}

	private IEnumerator PlayMountainLectureNextLevel() {
		DisableMovement (); DisableLaserGun ();
		pm.EnablePortals ();

		MountainLectureNextLevel.Play ();
		yield return new WaitForSeconds (MountainLectureNextLevel.clip.length);


		EnableMovement ();
		yield return null;
	}

	void EnableMountainColliderHalo() {
		if (!mountainHeightMeasured) {
			Behaviour gameObjectHalo = (Behaviour)mountainCollider.GetComponent ("Halo");
			gameObjectHalo.enabled = true;
		}
	}

	void EnableLaserGun() {
		interactionDistance = 50000;
		laserGun.SetActive (true);
	}

	void DisableLaserGun() {
		interactionDistance = 3;
		laserGun.SetActive (false);
	}

}
