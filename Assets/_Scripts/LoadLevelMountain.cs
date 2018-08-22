using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelMountain : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		Debug.Log ("Loading Mountain Level");
		SceneManager.LoadScene("Mountain HMD");
	}

}
