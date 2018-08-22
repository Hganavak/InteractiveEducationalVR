using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlightable : MonoBehaviour {

	public bool isHighlightable = true;

	private Color startcolor;

	void OnMouseEnter()
	{
		if (isHighlightable) {
			startcolor = GetComponent<Renderer> ().material.color;
			GetComponent<Renderer> ().material.color = Color.green;
		}
	}

	void OnMouseExit()
	{
		if (isHighlightable) {
			GetComponent<Renderer> ().material.color = startcolor;
		}
	}

}
