using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelMemoryPalace : MonoBehaviour {
	void OnTriggerEnter() {
		SceneManager.LoadScene("MemoryPalaceUnderground HMD");
	}
}
