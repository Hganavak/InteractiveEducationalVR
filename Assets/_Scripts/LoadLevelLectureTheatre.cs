using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelLectureTheatre : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		Debug.Log ("Loading Lecture Theatre Level");
		SceneManager.LoadScene("LectureTheatre HMD");
	}

}
