using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{

    public int Healthy;
    public int noofhearts;
    public int coins;
    public GameObject Wintext;
    public Image[] hearts;
    public Sprite fillheart;
    public Sprite emptyheart;
    public Text Coins;
    // Start is called before the first frame update
    void Start()
    {
        Healthy = (int)GameMaster.health;
        noofhearts = (int)GameMaster.maxHealth;
        Coins = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        coins = GameMaster.coins;
        noofhearts = (int)GameMaster.maxHealth;
        Healthy = (int)GameMaster.health;
        if (Healthy>noofhearts)
        {
            Healthy = noofhearts;
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i<Healthy)
            {
                hearts[i].sprite = fillheart;
            }
            else
            {
                hearts[i].sprite = emptyheart;
            }
            if(i<noofhearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        if (coins < 10)
        {
            Coins.text = "00" + coins.ToString() + "<color=grey><i>x</i></color>";
        }
        else if (coins > 10 && coins < 100)
        {
            Coins.text = "0" + coins.ToString() + "<color=grey><i>x</i></color>";
        }
        else
        {
            Coins.text = coins.ToString() + "<color=grey><i>x</i></color>";
        }
        if(GameMaster.levelwon)
        {
            Wintext.SetActive(true);
        }
    }
}
