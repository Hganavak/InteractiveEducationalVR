using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelPlayground : MonoBehaviour {
	void OnTriggerEnter() {
		SceneManager.LoadScene("Playground HMD");
	}
}
