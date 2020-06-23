using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour
{
    SceneManager sceneManager;
    public string nextScene;
	public AudioController SEPlayer;
    Button btn;

    // Start is called before the first frame update
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(LoadScene);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadScene()
    {
        SEPlayer.playSE(3);
        SceneManager.LoadScene(nextScene);
    }
}
