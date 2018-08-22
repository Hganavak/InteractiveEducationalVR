using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerLectureTheatrePC : SceneManagerPC {

	public PortalMasterPC pm;

	//Lectures
	public AudioSource lectureTheatreLectureIntroductionAndTaskDescription;
	public AudioSource lectureTheatreLectureNextLevel;

	//Game Objects
	public GameObject number1;
	public GameObject number2;
	public GameObject number3;
	public GameObject shape1;
	public GameObject shape2;
	public GameObject shape3;

	IEnumerator Start() {
		Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;

		PlayerPrefs.SetInt ("lectureTheatreVisited", 1);

		//Set initial progress bar
		UpdateProgress ();

		DisableMovement ();

		//Introduction lecture
		lectureTheatreLectureIntroductionAndTaskDescription.Play ();
		yield return new WaitForSeconds(lectureTheatreLectureIntroductionAndTaskDescription.clip.length);

		StartCoroutine (showInformationWithDelay (4f)); 

		yield return null;
	}

	IEnumerator showInformationWithDelay(float delay) {
		yield return new WaitForSeconds (2f);
		//Numbers
		number1.SetActive (true); ding.Play(); yield return new WaitForSeconds (delay); 
		number2.SetActive (true); ding.Play(); yield return new WaitForSeconds (delay); 
		number3.SetActive (true); ding.Play(); yield return new WaitForSeconds (delay); 
		yield return new WaitForSeconds (5f);
		number1.SetActive (false); number2.SetActive (false); number3.SetActive (false); 
		yield return new WaitForSeconds (2f);

		//Shapes
		shape1.SetActive (true); ding.Play(); yield return new WaitForSeconds (delay); 
		shape2.SetActive (true); ding.Play(); yield return new WaitForSeconds (delay); 
		shape3.SetActive (true); ding.Play(); yield return new WaitForSeconds (delay); 
		yield return new WaitForSeconds (5f);
		shape1.SetActive (false); shape2.SetActive (false); shape3.SetActive (false); 

		UpdateProgress (20); UpdateScore (200);

		//Enable movement and the portals
		pm.EnablePortals(); 

		lectureTheatreLectureNextLevel.Play ();
		yield return new WaitForSeconds (lectureTheatreLectureNextLevel.clip.length);

		EnableMovement();

		yield return null;
	}

}
