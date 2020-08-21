using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuScript : MonoBehaviour
{
    private void Update()
    {
        GameManager.instance.CheckForVolumeChange();
        SetVolume();
    }
    public void SetVolume()
    {
        //set audio listener volume to master volume
        AudioListener.volume = GameManager.instance.masterVolume;
        //set game music volume equal to user set music volume
        GameManager.instance.music.volume = GameManager.instance.musicVolume;
        //for every sound effect in the list,
        foreach (AudioSource _sound in GameManager.instance.sfx) 
        {
            //set its volume equal to user set sfx volume
            _sound.volume = GameManager.instance.sfxVolume;
        }
    }
}
