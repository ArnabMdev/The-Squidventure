using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlat : MonoBehaviour
{
    private Rigidbody2D rbd;
    public Transform startLoc;
    public Vector2 respawnpoint;
    // Start is called before the first frame update
    void Start()
    {
        startLoc = GetComponent<Transform>().transform;
        rbd = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            /*collision.collider.transform.SetParent(transform);*/
            Invoke("DropPlat", 1f);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        /*collision.collider.transform.SetParent(null);*/
    }

    void DropPlat()
    {
        rbd.isKinematic = false;
        /*Invoke("respawn", 2f);*/
    }

    public void respawn()
    {
        rbd.isKinematic = true;
        rbd.velocity = new Vector2(0f, 0f);
        transform.position = respawnpoint;
        transform.rotation = startLoc.rotation;
    }
}
