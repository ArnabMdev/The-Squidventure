using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public Animator anim;
    private bool triggered = false;
    private bool isopened = false;
    public enum animations
    {
        idle = 0,
        moving= 1,
        open = 2
    }
    public animations curranim;
    float n = 0f;
    void Start()
    {
        ChangeAnim(animations.idle);    
    }

    private void FixedUpdate()
    {
        
        n += Time.deltaTime;
        if(triggered)
        {
            if(n>=0.25f)
            {
                ChangeAnim(animations.open);
                isopened = true;
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isopened)
        {
            return;
        }
        if (collision.tag == "Player")
        {

            ChangeAnim(animations.moving);
            n = 0f;
            triggered = true;
            Debug.Log("chest opened");
        }
        
    }

    public void ChangeAnim(animations newAnim)
    {
        if (curranim != newAnim)
        {
            curranim = newAnim;
            anim.SetInteger("state", (int)curranim);
        }
    }
}
