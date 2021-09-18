using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerMotion : MonoBehaviour
{
    private bool underwater=false;
    public float velocity;
    public float jumpforce;
    [Range(0,1)]public float horizontaldamping=0;
    private CapsuleCollider2D cc;
    [SerializeField]
    private PhysicsMaterial2D nofric;
    [SerializeField]
    private PhysicsMaterial2D fullfric;
    private float i, j,k;
    private float fallMultiply=2.5f;
    private float lowJumpMultiply=2f;

    private Rigidbody2D rbd;
    private bool isgrounded;
    private float isgroundedremember;
    [SerializeField]private bool iswater;
    public Transform groundcheck;
    public Transform watercheck;
    public float checkradius;
    public LayerMask whatIsGround;
    public LayerMask whatisWater;

    public SpriteRenderer spr;
    private Animator anim;
    private bool isjumping;
    private bool isfalling;
    private float lastypos=0;
    private float jumpRememberTime;

    public AudioSource jumpSound;
    public AudioSource deathSound;
    public AudioSource coinSound;
    public AudioSource Bgm;

    public FallingPlat[] plats;
    public enum Animations
    {
        idle = 0,
        jump =1,
        fall = 2,
        run = 3,
        swim = 4,
        swimleft =5
    }
    public Animations currentanim;
    public bool UnderwaterLevel;

    private Transform flagloco;
    private bool wonlevel = false;
    private bool canmove = true;

    private bool active = true;
    private bool respawning = false;
    private float timetorespawn = 2;
    private float respawntime = 0;
    private Vector2 startpos;

    private float currTime = 0;
    private float maxTime = 2f;
    private bool runningtimer;
    public Vector2 collidersize;
    public float slopeCheckDistance;
    private float slopedownAngle;
    private Vector2 slopeNormalPerp;
    private bool isOnSlope;
    private float SlopeDownAngleOld;
    private float SlopeSideAngle;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        lastypos = transform.position.y;
        rbd = GetComponent<Rigidbody2D>();
        rbd.velocity = new Vector2(0f, 0f);
        active = true;
        currentanim = Animations.idle;
        anim = GetComponent<Animator>();
        ChangeAnim(Animations.idle);
        jumpRememberTime = 0f;
        isgroundedremember = 0f;
        cc = GetComponent<CapsuleCollider2D>();
        collidersize = cc.size;
    }
    void Update()
    {
        if(Input.GetKeyDown("escape") && !paused)
        {
            PauseGame();
            paused = true;
        }
        else if(Input.GetKeyDown("escape") && paused)
        {
            ResumeGame();
            paused = false;
        }
        //respawning
        if(respawning)
        {
            respawntime += Time.deltaTime;
            if(respawntime >=timetorespawn)
            {
                respawntime = 0;
                respawning = false;
                respawnplayer();
            }
        }
        if (GameMaster.health <= 0)
        {
            SceneManager.LoadScene("Game Over Scene");
        }
        if (!active || !canmove)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRememberTime = 0.2f;
        }
        //input
        i = Input.GetAxis("Horizontal");
        j = Input.GetAxis("Vertical");
        k = Input.GetAxis("Jump");


    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //respawn delay
        if (runningtimer)
        {
            currTime += Time.deltaTime;
            if (currTime >=maxTime)
            {
                runningtimer = false;
                active = true;
                /*Bgm.Play();*/
                rbd.isKinematic = false;
                transform.position = startpos;
                transform.rotation = new Quaternion(0, 0, 0, 0);
                foreach (FallingPlat plat in plats)
                {
                    plat.respawn();
                }
            }
        }

        //level win.
        if (wonlevel)
        {
            if(transform.position.x != flagloco.position.x)
            {
                ChangeAnim(Animations.run);
                Vector2.MoveTowards(transform.position, new Vector2(flagloco.position.x, flagloco.position.y), 1.8f);

            }
            if(Mathf.Abs(transform.position.x - flagloco.position.x)<=1)
            {
                ChangeAnim(Animations.idle);
                rbd.velocity = new Vector2(0f,0f);  
                GameMaster.WonLevel();
                wonlevel = false;

            }
        }

        //dead or wonlevel.
        if(!active || !canmove)
        {
            return;
        }

        checkSlope();

        //jumping or falling for anims.
        if(transform.position.y>lastypos && !isgrounded)
        {
            isjumping = true;
            isfalling = false;
        }
        else if(transform.position.y<lastypos && !isgrounded)
        {
            isjumping = false;
            isfalling = true;
        }
        else
        {
            isjumping = false;
            isfalling = false;
        }

        if(isOnSlope)
        {
            isjumping = false;
            isfalling = false;
        }

        
        
        //ground and watercheck
        isgrounded = Physics2D.OverlapCircle(groundcheck.position, checkradius, whatIsGround);
        iswater = Physics2D.OverlapCircle(watercheck.position, checkradius, whatisWater);
        underwater = UnderwaterLevel;

        if (isgrounded)
        {
            isgroundedremember = 0.2f;
        }


        //input to motion

        Vector2 newvel = new Vector2(0f, 0f);   
       /* horizontalvel *= Mathf.Pow(1f - horizontaldamping, Time.deltaTime * velocity);*/
       if(isgrounded && !isOnSlope && !underwater)
       {
            newvel.Set(velocity * i, rbd.velocity.y);
            rbd.velocity = newvel;

       }
       else if(isgrounded && isOnSlope && !underwater)
       {
            newvel.Set(velocity * slopeNormalPerp.x * -i, velocity * slopeNormalPerp.y * -i);
            rbd.velocity = newvel;
       }
       else if (!isgrounded && !underwater)
       {
            newvel.Set(i * velocity, rbd.velocity.y);
            rbd.velocity = newvel;
       }

        jumpRememberTime -= Time.deltaTime;
        isgroundedremember -= Time.deltaTime;
        if ((jumpRememberTime>0) && (isgroundedremember>0) && !underwater)
        {
            jumpRememberTime =0;
            isgroundedremember = 0f;
            rbd.velocity = Vector2.up * jumpforce;
            jumpSound.Play();
        }

        if(rbd.velocity.y<0)
        {
            rbd.gravityScale = fallMultiply;
        }
        else if(rbd.velocity.y>0 && !Input.GetButton("Jump"))
        {
            rbd.gravityScale = lowJumpMultiply;
        }
        else
        {
            rbd.gravityScale = 1f;
        } 
        

        //flip
        if (i > 0 && !underwater && !iswater)
        {
            spr.flipX = false;
        }
        else if (i < 0 && !underwater && !iswater)
        {
            spr.flipX = true;
        }

        //animations
        if (isjumping && !underwater)
        {
            ChangeAnim(Animations.jump);
        }
        else if(isfalling && !underwater)
        {
            ChangeAnim(Animations.fall);
        }
        else if (rbd.velocity.x != 0 &&isgrounded && !underwater)
        {
            ChangeAnim(Animations.run);
        }
        else if(Mathf.Round(rbd.velocity.x) == 0 && rbd.velocity.y==0 && !underwater)
        {
            ChangeAnim(Animations.idle);
        }

        //fall
        if (transform.position.y <= -6.81f)
        {
            death();
            Debug.Log("y");
            GameMaster.TakeDamage();
        }

        //water movement
        if (iswater)
        {
            swim();

        }
        lastypos = transform.position.y;


        //Underwater movement
        if (underwater)
        {
            spr.flipX = false;
            float Horiwim = i * velocity;
            float Vertiswim;
            if (Mathf.Abs(j) > Mathf.Abs(k))
            {
                Vertiswim = j * velocity * 0.5f;
            }
            else
            {
                Vertiswim = k * velocity * 0.5f;

            }
            if (i > 0)
            {
                ChangeAnim(Animations.swim);
            }
            else
            {
                ChangeAnim(Animations.swimleft);
            }
            rbd.gravityScale = 1.3f;
            rbd.velocity = new Vector2(Horiwim, Vertiswim);
        }
    }

    private void checkSlope()
    {
        Vector2 checkpos = transform.position - new Vector3(0f, collidersize.y / 2);
        slopecheckHorizontal(checkpos);
        slopecheckVertical(checkpos);
    }

    private void slopecheckHorizontal( Vector2 checkpos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkpos,transform.right,slopeCheckDistance,whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkpos,-transform.right,slopeCheckDistance,whatIsGround);
        if(slopeHitFront)
        {
            isOnSlope = true;
            SlopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
      
        }
        else if(slopeHitBack)
        {
            isOnSlope = true;
            SlopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            isOnSlope = false;
            SlopeSideAngle = 0.0f;
        }
    }

    private void slopecheckVertical(Vector2 checkpos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkpos, Vector2.down,slopeCheckDistance,whatIsGround);
        if(hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
            slopedownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopedownAngle!=SlopeDownAngleOld)
            {
                isOnSlope = true;
            }

            SlopeDownAngleOld = slopedownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal,Color.black);
        }
        if(isOnSlope && (i == 0f))
        {
            rbd.sharedMaterial = fullfric;
        }
        else
        {
            rbd.sharedMaterial = nofric;
        }


    }

    private void swim()
    {
        spr.flipX = false;

        /*spr.transform.rotation = new Quaternion(0f, 0f, 90f, 0f);*/
        k = Mathf.Clamp(k, 0, 0.8f);
        j = Mathf.Clamp(j, -0.8f, 0.8f);
        if (Mathf.Abs(j) > Mathf.Abs( k))
        {
            rbd.velocity = new Vector2(rbd.velocity.x, j * 7f);
        }
        else
        {
            rbd.velocity = new Vector2(rbd.velocity.x, k * 7f);

        }
        if (rbd.velocity.x > 0)
        {
            ChangeAnim(Animations.swim);
        }
        else if(rbd.velocity.x<=0)
        {
            ChangeAnim(Animations.swimleft);
        }
    }
    public void death()
    {
        Bgm.Stop();

        active = false;
        rbd.isKinematic = true;
        rbd.velocity = Vector2.zero;
        runningtimer = true;
        currTime = 0;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        deathSound.Play();
        
       
    }

    public void respawnplayer()
    {
        active = true;
        rbd.isKinematic = false;
        transform.position = startpos;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            GameMaster.coins++;
            Destroy(collision.gameObject);
            coinSound.Play();
        }

        if (collision.gameObject.tag == "Underwater")
        {
            underwater = true;
        }
        else 
        {
            underwater = false;
        }
    }

    public void ChangeAnim(Animations newAnim)
    {
        if(currentanim != newAnim)
        {
            currentanim = newAnim;
            anim.SetInteger("state", (int)currentanim);
        }
    }

    public void Wonlevel(Transform flagloc)
    {
        canmove = false;
        flagloco = flagloc;
        wonlevel = true;


    }
    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }


}
