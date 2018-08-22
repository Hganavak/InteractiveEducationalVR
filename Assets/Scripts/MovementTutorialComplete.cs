using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTutorialComplete : MonoBehaviour {

	public SceneManagerTutorialPC sceneManagerTutorialPC;

	void OnTriggerEnter(Collider col) {
		Debug.Log ("Collided with: " + col.transform.name);
		if (!sceneManagerTutorialPC.movementTutorialCompleted) {
			StartCoroutine(sceneManagerTutorialPC.MovementTutorialComplete());
		}
	}
}
