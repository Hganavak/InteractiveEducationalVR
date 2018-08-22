using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontCollideWithPickupables : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.GetComponent<Pickupable> () != null) {
			Debug.Log ("Character collided pickupable: " + col.gameObject.transform.name);
			Physics.IgnoreCollision(col, GetComponent<Collider>());
		}
	}

}
