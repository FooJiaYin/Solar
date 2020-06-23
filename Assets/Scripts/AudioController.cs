using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip launchSE, levelUpSE, changeSceneSE, hitSE, clickSE, wrongSE;
    private AudioSource audioPlayer;
    void Awake() 
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        audioPlayer = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSE(int type) {
        if(type == 1) {
            audioPlayer.PlayOneShot(launchSE);
        }
        else if(type == 2) {
            audioPlayer.PlayOneShot(levelUpSE);
        }
        else if(type == 3) {
            audioPlayer.PlayOneShot(changeSceneSE);
        }
        else if(type == 4) {
            audioPlayer.PlayOneShot(hitSE);
        }
        else if(type == 5){
            audioPlayer.PlayOneShot(clickSE);
        }
        else if(type == 6){
            audioPlayer.PlayOneShot(wrongSE);
        }
    }
}
