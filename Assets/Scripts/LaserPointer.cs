using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LaserPointer : VRTK_Pointer {


	public void OnPointerEnter(RaycastHit givenHit) {
		Debug.Log ("hit something valid");
	}
}
