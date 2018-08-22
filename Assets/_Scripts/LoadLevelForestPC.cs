using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelForestPC : MonoBehaviour {
	void OnTriggerEnter() {
		Debug.Log ("Loading Forest Scene (PC)...");
		SceneManager.LoadScene("ForestPC");
	}
}
