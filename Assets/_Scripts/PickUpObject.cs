using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Test
public class PickUpObject : MonoBehaviour {
	GameObject mainCamera;
	bool carrying;
	GameObject carriedObject;
	public float carryDistance;
	public float smooth;
	public float distanceFromObject;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindWithTag("MainCamera");
	}

	// Update is called once per frame
	void Update () {
		if(carrying) {
			carry(carriedObject);
			checkDrop();
			//rotateObject();
		} else {
			pickup();
		}
	}

	void rotateObject() {
		carriedObject.transform.Rotate(5,10,15);
	}

	void carry(GameObject o) {
		o.transform.position = Vector3.Lerp (o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * carryDistance, Time.deltaTime * smooth);
	}

	void pickup() {
		if(Input.GetMouseButton(0)) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit) && Vector3.Distance(mainCamera.transform.position, hit.transform.position) < distanceFromObject) {
				//Debug.Log("Distance to object: " + Vector3.Distance(mainCamera.transform.position, hit.transform.position));
				Pickupable p = hit.collider.GetComponent<Pickupable>();
				if(p != null) {
					carrying = true;
					carriedObject = p.gameObject;
					p.gameObject.GetComponent<Rigidbody>().isKinematic = true;
				}
			}
		}
	}

	void checkDrop() {
		if(!Input.GetMouseButton(0)) {
			dropObject();
		}
	}// 

	void dropObject() {
		carrying = false;
		carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		carriedObject = null;
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		Debug.Log ("Controller collider hit something");
	}



}

