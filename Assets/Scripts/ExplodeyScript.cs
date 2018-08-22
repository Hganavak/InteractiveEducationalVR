
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRTK;


public class ExplodeyScript : VRTK_InteractableObject
{
	
    protected void Start()
    {

    }

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
		PlayerPrefs.SetInt("progress", PlayerPrefs.GetInt("progress")+1);
		Slider progressBar = GameObject.Find ("ProgressBar").GetComponent<Slider> ();
		progressBar.value = PlayerPrefs.GetInt ("progress");
    }


}

