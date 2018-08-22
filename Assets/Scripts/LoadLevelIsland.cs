using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelIsland : MonoBehaviour {
	
	void OnTriggerEnter(Collider col) {
		Debug.Log ("TRIGGAD");
		SceneManager.LoadScene("Island HMD");
	}

}
