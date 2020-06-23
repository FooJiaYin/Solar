using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetOnClick : MonoBehaviour
{
    Button btn;
	public AudioController SEPlayer;
    // Start is called before the first frame update
    void Start()
    {
        btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(ResetAll);
    }

    // Update is called once per frame
    void Update()
    {    

    }

    public void ResetAll() {
        Debug.Log("reset");
        GameObject[] Planets = GameObject.FindGameObjectsWithTag("Planet");
        foreach(GameObject planet in Planets) {
            Debug.Log("reset " + planet.name);
            if(planet) planet.GetComponent<PlanetController>().ResetPlanet();
        }
        GameObject moon = GameObject.FindGameObjectWithTag("Moon");
        if(moon) {
            Debug.Log("reset " + moon.name);
            moon.GetComponent<PlanetController>().ResetPlanet();
        }
        GameObject.Find("count text").GetComponent<ScoreDisplay>().Init();
        SEPlayer.playSE(3);
        GameObject.Find("next button").SetActive(false);
    }
}
