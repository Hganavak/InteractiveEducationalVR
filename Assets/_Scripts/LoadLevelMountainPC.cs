using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelMountainPC : MonoBehaviour {
	void OnTriggerEnter() {
		Debug.Log ("Loading Mountain Scene (PC)...");
		SceneManager.LoadScene("MountainPC");
	}
}
