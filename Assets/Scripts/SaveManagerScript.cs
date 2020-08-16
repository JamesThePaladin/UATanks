using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManagerScript : MonoBehaviour
{
    public Text theText;
    public void Save() 
    {
        PlayerPrefs.SetString("TextData", theText.text);
        PlayerPrefs.Save();
    }
    public void Load()
    {
        theText.text = PlayerPrefs.GetString("TextData");
    }
}
