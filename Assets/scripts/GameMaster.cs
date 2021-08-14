 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster :MonoBehaviour
{

    public static bool runningtransitiontimer = false;
    public float currTransitionTimer = 0;
    public float timeToTransition = 2f;
    public static string currentlevel = "Level1";
    public string  nextlevel = "Level2";
    public static float health = 3;
    public static float maxHealth = 5;
    public static int coins = 0;
    public static bool levelwon = false;

    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void  Update()
    {
        if(runningtransitiontimer)
        {
            currTransitionTimer += Time.deltaTime;
            if(currTransitionTimer>=timeToTransition)
            {
                currTransitionTimer = 0;
                runningtransitiontimer = false;
                levelwon = false;
                currentlevel = nextlevel;
                SceneManager.LoadScene(nextlevel);
                

            }
        }
        if (health <= 0f)
        {
            health += 3;
            SceneManager.LoadScene("Game Over Scene");

        }

    }


    public static void WonLevel()
    {
        
        GameMaster.runningtransitiontimer = true;
        levelwon = true;

    }

    public static void WonLevelTransition()
    {
        GameMaster.runningtransitiontimer = true;
    }

    public static void Addhealth()
    {
        GameMaster.maxHealth += 1;
    }

    public static void TakeDamage()
    {
        GameMaster.health--;
        Mathf.Clamp(GameMaster.health, 0, GameMaster.maxHealth);
        Debug.Log(health);
    }

    public static void Heal()
    {
        GameMaster.health++;
        Mathf.Clamp(GameMaster.health, 0, GameMaster.maxHealth);
    }
}
