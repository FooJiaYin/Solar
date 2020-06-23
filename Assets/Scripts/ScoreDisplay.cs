using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {
	public int initialValue = 0;
	public string prefix = "";
	// public string TargetSceneName = "";
	// public bool changeScene;
	// public int boundary;
    int value = 0;
    Text numText;

	/* Display Message */
	string sceneName;
	public GameObject messageObject;
	public GameObject LevelUpObject;
	public GameObject nextButton;
	public AudioController SEPlayer;
    // Use this for initialization
	void Start () {
		numText = GetComponent<Text>();
		sceneName = SceneManager.GetActiveScene().name;
		//messageObject = GameObject.Find("message");
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			if(messageObject.activeSelf) messageObject.SetActive(false);
			if(LevelUpObject.activeSelf) LevelUpObject.SetActive(false);
		}
	}

	void Display() {
		numText.text = prefix + value.ToString();
	}

	public void Add(int num) {
		value += num;
		Display();
		// if(changeScene && value == boundary) SceneManager.LoadScene(TargetSceneName);
	}

	public int GetValue() {
		return value;
	}

	public void Init() {
		value = initialValue;  
		messageObject.SetActive(false);
		Display();
	}

	public void Judge(GameObject planet, GameObject center) {
		float distance = Vector3.Distance(planet.transform.position, center.transform.position);
		Debug.Log(distance);
		// switch(sceneName) {
		// 	case "Level 4":
				if(center.name == "sun") {
					if(planet.GetComponent<PlanetController>().planetPrefab.name == "earth body") {
						if(distance > 1.85) LevelUp();
						else {
							DisplayMessage("Too hot!!\nThe earth is too close to the sun.");
							SEPlayer.playSE(6);
							planet.GetComponent<PlanetController>().ResetPlanet();
							GameObject.FindGameObjectWithTag("Moon").GetComponent<PlanetController>().ResetPlanet();
						}
					}
					else {
						DisplayMessage("The moon should orbit around the earth");
						SEPlayer.playSE(6);
						planet.GetComponent<PlanetController>().ResetPlanet();
					}
				}
				else {
					if(distance < 1 || (sceneName == "Level 1" && distance < 1.8)) {
						DisplayMessage("The moon is too close to the earth.");
						SEPlayer.playSE(6);
						planet.GetComponent<PlanetController>().ResetPlanet();
					}
					else if(sceneName == "Level 1") LevelUp();
					else SEPlayer.playSE(2);
				}
		// 	break;
		// }
	}

	public void DisplayMessage(string msg) {
		messageObject.transform.Find("message text").GetComponent<Text>().text = msg;
		messageObject.SetActive(true);
	}

	public void LevelUp() {
		string grade;
		switch(sceneName) {
		 	case "Level 1":
				if(value <= 10) grade = "★★★★★";
				else if(value <= 15) grade = "★★★★☆";
				else if(value <= 20) grade = "★★★☆☆";
				else if(value <= 25) grade = "★★☆☆☆";
				else if(value <= 30) grade = "★☆☆☆☆";
				else grade = "☆☆☆☆☆";
			break;
			case "Level 2":
				if(value <= 10) grade = "★★★★★";
				else if(value <= 20) grade = "★★★★☆";
				else if(value <= 30) grade = "★★★☆☆";
				else if(value <= 40) grade = "★★☆☆☆";
				else if(value <= 50) grade = "★☆☆☆☆";
				else grade = "☆☆☆☆☆";
			break;
			case "Level 3":
				if(value <= 10) grade = "★★★★★";
				else if(value <= 20) grade = "★★★★☆";
				else if(value <= 30) grade = "★★★☆☆";
				else if(value <= 40) grade = "★★☆☆☆";
				else if(value <= 50) grade = "★☆☆☆☆";
				else grade = "☆☆☆☆☆";
			break;
			case "Level 4":
				if(value <= 15) grade = "★★★★★";
				else if(value <= 30) grade = "★★★★☆";
				else if(value <= 50) grade = "★★★☆☆";
				else if(value <= 70) grade = "★★☆☆☆";
				else if(value <= 90) grade = "★☆☆☆☆";
				else grade = "☆☆☆☆☆";
			break;
			default:
				grade = "★★★★★";
			break;
		}
		LevelUpObject.transform.Find("grade text").GetComponent<Text>().text = "Score    " + grade;
		LevelUpObject.SetActive(true);
		nextButton.SetActive(true);
		SEPlayer.playSE(2);
	}
}