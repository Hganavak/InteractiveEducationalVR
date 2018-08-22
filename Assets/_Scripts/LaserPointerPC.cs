 using UnityEngine;
 using System.Collections;
 
 public class LaserPointerPC : MonoBehaviour
 {
    public LineRenderer laserLineRenderer;
	private RaycastHit objectHit;
	public bool LaserActive;
 
	void Update() 
	{
		if( Input.GetMouseButton (0) ) {
			laserLineRenderer.useWorldSpace = false;

			LaserActive = true;
			laserLineRenderer.enabled = true;

			Debug.DrawRay(transform.position, transform.forward * 1000, Color.green);

			Physics.Raycast(transform.position,transform.forward, out objectHit);
			if(objectHit.collider){
				laserLineRenderer.SetPosition(1, new Vector3(0,0,objectHit.distance));
			}
			else{
				laserLineRenderer.SetPosition(1, new Vector3(0,0,5000));
			}
		}
		else {
			LaserActive = false;
			laserLineRenderer.enabled = false;
		}
	}

	//
	/*
     void Update() 
     {
		if( Input.GetMouseButton (0) ) {
			LaserActive = true;
			laserLineRenderer.enabled = true;

			Debug.DrawRay(transform.position, transform.forward * 1000, Color.green);

			laserLineRenderer.SetPosition (0, transform.position);
			laserLineRenderer.SetPosition (1, transform.forward*1000);
         }
         else {
			LaserActive = false;
            laserLineRenderer.enabled = false;
         }
     }*/
 
 }