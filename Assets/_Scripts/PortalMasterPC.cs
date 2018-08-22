using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMasterPC : MonoBehaviour {

	//Portals: 4 portals, each with 2 versions
	public GameObject forestPortalEnabled;
	public GameObject forestPortalDisabled;
	public GameObject lectureTheatrePortalEnabled;
	public GameObject lectureTheatrePortalDisabled;
	public GameObject mountainPortalEnabled;
	public GameObject mountainPortalDisabled;
	public GameObject memoryPalacePortalEnabled;
	public GameObject memoryPalacePortalDisabled;

	/*
	 * Checks player prefs, and decides which of the 2 variants of each portal to show
	 */
	public void EnablePortals() {

		//Forest
		if (PlayerPrefs.GetInt ("forestVisited") == 0) {
			forestPortalEnabled.SetActive (true);
		} else {
			forestPortalDisabled.SetActive (true);
		}

		//Lecture Theatre
		if (PlayerPrefs.GetInt ("lectureTheatreVisited") == 0) {
			lectureTheatrePortalEnabled.SetActive (true);
		} else {
			lectureTheatrePortalDisabled.SetActive (true);
		}

		//Mountain
		if (PlayerPrefs.GetInt ("mountainVisited") == 0) {
			mountainPortalEnabled.SetActive (true);
		} else {
			mountainPortalDisabled.SetActive (true);
		}

		//Memory Palace
		if (PlayerPrefs.GetInt ("forestVisited") == 1 && PlayerPrefs.GetInt ("lectureTheatreVisited") == 1 && PlayerPrefs.GetInt ("mountainVisited") == 1) {
			memoryPalacePortalEnabled.SetActive (true);
		} else {
			memoryPalacePortalDisabled.SetActive (true);
		}

	}
}
