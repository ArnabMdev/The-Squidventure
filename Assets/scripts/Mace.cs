using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Mace : MonoBehaviour
{
    private Rigidbody2D rbd;
    private Vector2 startPos;
    public Transform MaceTransform;
    public float animDur;
    public Ease animEase;
    public float Target;
    private float timeSinceCrush;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceCrush = 2 * animDur;
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        timeSinceCrush += Time.deltaTime;
   
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && timeSinceCrush >=2*animDur)
        {
            MaceTransform.DOMoveY(Target, animDur);
            MaceTransform.DOMoveY(startPos.y, animDur*1.5f  ).SetDelay(animDur);
            timeSinceCrush = 0f;
        }


    }
}
