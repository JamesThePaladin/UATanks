using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject optionsMenuUI;
    public GameObject playerSelectUI;

    public void PlaySinglePlayer()
    {
        GameManager.instance.isMultiplayer = false;
        GameManager.instance.isGameStart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.instance.GameStart();
    }

    public void PlayMultiplayer() 
    {
        GameManager.instance.isGameStart = true;
        GameManager.instance.isMultiplayer = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenPlayerSelect() 
    {
        playerSelectUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void ClosePlayerSelect() 
    {
        mainMenuUI.SetActive(true);
        playerSelectUI.SetActive(false);
    }

    public void OpenOptions()
    {
        optionsMenuUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void CloseOptions() 
    {
        mainMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }


    //for quit button on start menu, closes the app
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChooseRandomMap() 
    {
        GameManager.instance.isRandomMap = true;
        GameManager.instance.isMapOfTheDay = false;
    }

    public void ChooseMOtD() 
    {
        GameManager.instance.isRandomMap = false;
        GameManager.instance.isMapOfTheDay = true;
    }
}
