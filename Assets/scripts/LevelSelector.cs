using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public Button[] buttons;
    
    void Start()
    {
        if(buttons.Length!=0)
        {
            AvailableLevels();
        }

    }

    private void AvailableLevels()
    {
        for (int i = 0; i < GameMaster.levelNo; i++)
        {
            buttons[i].interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(GameMaster.currentlevel);
        GameMaster.health = 3;
        PlayerPrefs.SetFloat("SavedHealth", 3f);
    }

    public void select(string levelname)
    {
        SceneManager.LoadScene(levelname);
    }

    public void levelselectscreen()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
