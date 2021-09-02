using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceKiller : MonoBehaviour
{
    public PlayerMotion player;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.death();
        }
    }
}
