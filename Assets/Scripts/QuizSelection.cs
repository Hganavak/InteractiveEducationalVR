using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class QuizSelection : VRTK_InteractableObject {

	public SceneManagerRoomHMD sceneManager;
	public AudioSource roomLectureIncorrectQuizSelection;
	public AudioSource roomLectureCorrectQuizSelection;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		if (this.gameObject.tag == "CorrectQuizSelection") {
			sceneManager.UpdateProgress(10); sceneManager.UpdateScore(25);
			Debug.Log ("Correct!");
			StartCoroutine ("playRoomLectureCorrectQuizSelection");
		} else {
			sceneManager.UpdateScore(-25);
			Debug.Log ("Incorrect!");
			StartCoroutine ("playRoomLectureIncorrectQuizSelection");
		}

	}

	IEnumerator playRoomLectureIncorrectQuizSelection() {
		isUsable = false;
		touchHighlightColor = Color.red; //Make the object red
		roomLectureIncorrectQuizSelection.Play(); //Lecture that explains the selection was incorrect
		yield return new WaitForSeconds(roomLectureIncorrectQuizSelection.clip.length); // Wait until clip has finished
	}

	IEnumerator playRoomLectureCorrectQuizSelection() {
		isUsable = false;
		touchHighlightColor = Color.yellow; //Make the object yellow
		roomLectureCorrectQuizSelection.Play(); //Lecture that explains the selection was correct
		yield return new WaitForSeconds(roomLectureCorrectQuizSelection.clip.length); // Wait until clip has finished
		sceneManager.NextLevel();
	}

}
