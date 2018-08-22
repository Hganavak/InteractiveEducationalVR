using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLaserPointerTutorial : MonoBehaviour {

	public SceneManagerTutorialPC sceneManagerTutorialPC;

	void OnTriggerEnter(Collider col) {
		Debug.Log ("Collided with: " + col.transform.name);
		if (sceneManagerTutorialPC.laserPointerTutorialDestinationTriggered && sceneManagerTutorialPC.ballTutorialCompleted) {
			StartCoroutine(sceneManagerTutorialPC.BeginLaserPointerTutorial());
			GameObject.Find("Ball").SetActive(false);
		}
	}
}
