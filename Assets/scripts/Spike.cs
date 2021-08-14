using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public PlayerMotion player;
    public Transform playertransform;
    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            GameMaster.TakeDamage();
            player.death();
            Debug.Log("x");
        }
       
    }

}
