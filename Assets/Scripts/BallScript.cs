
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRTK;


public class BallScript : VRTK_InteractableObject
{
	public SceneManagerTutorialHMD sceneManager;

	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		isUsable = false;
		StartCoroutine (sceneManager.InteractionTutorialCompleted ());
	}


}

