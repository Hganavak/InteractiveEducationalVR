using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PortalMaster : MonoBehaviour {

	public VRTK_DestinationPoint destinationPointForest;
	public VRTK_DestinationPoint destinationPointLectureTheatre;
	public VRTK_DestinationPoint destinationPointMountain;

	public VRTK_DestinationPoint destinationPointMemoryPalace;

		
	public void UpdatePortalStates() {

		//Disable any teleports that have already been visited
		if (PlayerPrefs.GetInt ("forestVisited") == 1)
			destinationPointForest.enableTeleport = false;
		if (PlayerPrefs.GetInt ("lectureTheatreVisited") == 1)
			destinationPointLectureTheatre.enableTeleport = false;
		if (PlayerPrefs.GetInt ("mountainVisited") == 1)
			destinationPointMountain.enableTeleport = false;

		//If all the destinations have been visited, enable the final level portal
		if (PlayerPrefs.GetInt ("forestVisited") == 1 && PlayerPrefs.GetInt ("lectureTheatreVisited") == 1 && PlayerPrefs.GetInt ("mountainVisited") == 1) {
			destinationPointMemoryPalace.enableTeleport = true;
		}

	}

}
