using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMotion : MonoBehaviour
{
    public PlayerMotion player;
    public GameObject playeri;

    public Transform playertransform;
    public bool swivelUpDown = false;
    public float upperlim;
    public float lowerlim;
    public bool swivelLeftRight = false;
    public float vel;
    Rigidbody2D rbody;


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        if(swivelUpDown)
        {
            rbody.velocity = new Vector2(0f, vel);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, 10f);
        if(swivelUpDown)
        {
            if (transform.position.y >= upperlim)
            {
                rbody.velocity = new Vector2(0f, -vel);
            }
            if (transform.position.y <= lowerlim)
            {
                rbody.velocity = new Vector2(0f, vel);

            }
        }
  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag!="Player")
        {
            return;
        }
        GameMaster.TakeDamage();
        player.death();
        return;
        
    }
}
