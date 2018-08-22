using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class TutorialActivator : VRTK_InteractableObject {

	public SceneManagerTutorialHMD sceneManager;

	public AudioSource tutorialLectureInteractionTutorialBall;

	public GameObject cornerLight;
	public GameObject ball;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		sceneManager.tutorialButtonPressed = true;
		Debug.Log ("You used the tutorial button");
		sceneManager.UpdateProgress (5);sceneManager.UpdateScore (50);
		isUsable = false;
		StartCoroutine (BallTutorial ());
	}
		
	IEnumerator BallTutorial() {
		
		tutorialLectureInteractionTutorialBall.Play ();
		yield return new WaitForSeconds(tutorialLectureInteractionTutorialBall.clip.length);

		cornerLight.SetActive (true);
		ball.SetActive (true);


		yield return null;
	}

}
