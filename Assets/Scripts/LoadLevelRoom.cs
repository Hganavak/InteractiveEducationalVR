using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelRoom : MonoBehaviour {
	void OnTriggerEnter() {
		SceneManager.LoadScene("Room HMD");
	}
}
