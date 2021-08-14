using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(GameMaster.currentlevel);
        GameMaster.health = 3;
    }

    public void select(string levelname)
    {
        SceneManager.LoadScene(levelname);
    }

    public void levelselectscreen()
    {
        SceneManager.LoadScene("LevelSelect");
    }

}
