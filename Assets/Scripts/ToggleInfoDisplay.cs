using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleInfoDisplay : MonoBehaviour
{
    Button btn;
    public GameObject InfoDisplay;
    public AudioController SEPlayer;

    // Start is called before the first frame update
    void Start()
    {
        btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(ToggleDisplay);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDisplay() {
        InfoDisplay.SetActive(!InfoDisplay.activeSelf);
        SEPlayer.playSE(5);
    }
}
