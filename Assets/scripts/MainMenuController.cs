using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource click;

    public void Continue()
    {
        click.Play();
        SceneManager.LoadScene("LevelSelect");
    }

    public void NewGame()  
    {
        click.Play();
        PlayerPrefs.SetInt("SavedLevel", 1);
        PlayerPrefs.SetInt("SavedCoins", 0);
        PlayerPrefs.SetFloat("SavedHealth", 3f);

        SceneManager.LoadScene("LevelSelect");

    }

    public void LevelSelect()
    {
        click.Play();
        SceneManager.LoadScene("Options");

    }

    public void Exit()
    {
        click.Play();
        Invoke("Application.Quit", 2f);
    }
}
