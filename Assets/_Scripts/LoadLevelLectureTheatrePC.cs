using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelLectureTheatrePC : MonoBehaviour {
	void OnTriggerEnter() {
		Debug.Log ("Loading Lecture Theatre Scene (PC)...");
		SceneManager.LoadScene("LectureTheatrePC");
	}
}
