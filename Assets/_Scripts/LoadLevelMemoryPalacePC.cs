using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelMemoryPalacePC : MonoBehaviour {
	void OnTriggerEnter() {
		Debug.Log ("Loading Underground Memory Palace Scene (PC)...");
		SceneManager.LoadScene("MemoryPalaceUndergroundPC");
	}
}
