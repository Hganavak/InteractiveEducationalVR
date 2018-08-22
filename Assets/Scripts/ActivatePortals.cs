namespace VRTK.Examples
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.UI;


	public class ActivatePortals : VRTK_InteractableObject
	{
		public SceneManagerTutorialHMD sceneManager;
		public GameObject portals;
		public Transform locationForPortalsToAppear;

		public override void StartUsing(VRTK_InteractUse usingObject)
		{
			sceneManager.NextLevel();
		}


	}
}
