﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour {

    GameObject mainCamera;

	// Use this for initialization
	void Start () {
        mainCamera = GameObject.FindWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "BlackSpiralShellfish")
                {
                    Debug.Log("You found the shellfish!");
                    Slider progressBar = GameObject.FindWithTag("ProgressBar").GetComponent<Slider>();
                    progressBar.value += 5;
                    //Destroy(hit.collider.gameObject);
                }
            }
        }
  
    }
}
