using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haloable : MonoBehaviour {

	public bool isHaloable = true;

	void OnMouseEnter() {
		if (isHaloable) {
			Behaviour gameObjectHalo = (Behaviour)GetComponent("Halo");
			gameObjectHalo.enabled = true;
		}
	}

	void OnMouseExit() {
		if (isHaloable) {
			Behaviour gameObjectHalo = (Behaviour)GetComponent("Halo");
			gameObjectHalo.enabled = false;
		}
	}
}
