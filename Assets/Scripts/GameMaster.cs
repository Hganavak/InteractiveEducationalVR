using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

	public static GameMaster GM;

	//Player Variables
	private int score;
	private int progress;

	//Location Variables
	public bool islandVisited;
	public bool playgroundVisited;
	public bool roomVisited;

	public static bool memoryPalaceUnlocked;

	//Create Singleton and tell Unity not to destroy this class between scenes
	void Awake() {

		if (GM != null) {
			GameObject.Destroy (this);
		} else {
			GM = this;
		}

		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () {
		score = 0;
		progress = 1;

		islandVisited = false;
		playgroundVisited = false;
		roomVisited = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Update the progress value and corresponding visual representation
	public void UpdateProgressBar() {
		progress += 30;
		Slider progressBar = GameObject.Find ("Slider").GetComponent<Slider>();
		progressBar.value = progress;
	}
}
