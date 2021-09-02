using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    private Rigidbody2D rbody;
    public float upperlim, lowerlim;
    public bool leftright;
    public bool onPath = false;[SerializeField]
    private Transform[] routes;
    private int routeToGo;
    private float tParam;
    private Vector2 objectPosition;
    [SerializeField] private float speedModifier;
    private bool coroutineAllowed;
    public bool fallOnTouch = false;

    // Start is called before the first frame update
    void Start()
    {

        rbody = GetComponent<Rigidbody2D>();
        if(!leftright && !onPath /*&& !fallOnTouch*/)
        {
            rbody.velocity = new Vector2(0f, 2f);
        }
        else if(onPath /*&&*/ /*!fallOnTouch*/)
        {
            routeToGo = 0;
            tParam = 0f;
            coroutineAllowed = true;
        }
        else if(leftright && !onPath /*&& !fallOnTouch*/)
        {
            rbody.velocity = new Vector2(2f, 0f);
        }

    }

    private void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(leftright &&!onPath)
        {
            if (transform.position.x >= upperlim)
            {
                rbody.velocity = new Vector2(-2f, 0f);
            }
            if (transform.position.x <= lowerlim)
            {
                rbody.velocity = new Vector2(2f, 0f);

            }
        }
        else if(!leftright && !onPath)
        {
            if (transform.position.y >= upperlim)
            {
                rbody.velocity = new Vector2(0f, -2f);
            }
            if (transform.position.y <= lowerlim)
            {
                rbody.velocity = new Vector2(0f, 2f);

            }
        }
       
    }
    private IEnumerator GoByTheRoute(int routeNum)
    {
        coroutineAllowed = false;

        Vector2 p0 = routes[routeNum].GetChild(0).position;
        Vector2 p1 = routes[routeNum].GetChild(1).position;
        Vector2 p2 = routes[routeNum].GetChild(2).position;
        Vector2 p3 = routes[routeNum].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;

        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }

        coroutineAllowed = true;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(transform);
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.collider.transform.SetParent(null);
    }
}
