using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelForest : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		Debug.Log ("Loading Forest Level");
		SceneManager.LoadScene("Forest HMD");
	}

}
