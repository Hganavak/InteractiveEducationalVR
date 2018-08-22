using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightShellfish : MonoBehaviour {

	public bool isHighlightable = true;
	public Material shellfishMaterial;
	private Color startcolor;

	void OnMouseEnter() {
		if (isHighlightable) {
			startcolor = shellfishMaterial.color;
			shellfishMaterial.color = Color.green;
		}
	}

	void OnMouseExit() {
		if (isHighlightable) {
			shellfishMaterial.color = startcolor;
		}
	}
}
