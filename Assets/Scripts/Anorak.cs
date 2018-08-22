using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anorak : MonoBehaviour {

	/*
	 * Class is used for setting up starting game values
	 */

	// Use this for initialization
	void Start () {
		PlayerPrefs.DeleteAll ();//RESET PLAYER PREFS
		PlayerPrefs.SetInt ("progress", 0);
		PlayerPrefs.SetInt ("score", 0);
		PlayerPrefs.SetInt ("forestVisited", 0);
		PlayerPrefs.SetInt ("lectureTheatreVisited", 0);
		PlayerPrefs.SetInt ("mountainVisited", 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
