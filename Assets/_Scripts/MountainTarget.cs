using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MountainTarget : VRTK_InteractableObject {

	public SceneManagerMountainHMD sceneManager;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		Debug.Log("You measured the mountain");

		pointerActivatesUseAction = false;
		isUsable = false;
		sceneManager.MountainMeasured ();
	}
}
