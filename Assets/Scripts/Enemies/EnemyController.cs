using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public abstract class EnemyController : MonoBehaviour
{
    GameController cont;
    public float hp = 50;
    public float maxHp = 50;
    public GameObject deadSquid;
    public float chaseDistance;
    public float attackRange;
    public Image health;
    public float lerpSpd = 5f;
    public float timeBetweenAttacks;
    //Current movement speed
    protected float curSpd;

    //AI stuff
    public Transform target;
    public float nextWaypointDistance = 2f;
    public float spd;
    public float slowSpd;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    protected Rigidbody2D bod;

    public enum states { idle, chase, attack };
    public states curState = states.idle;

    //Animating
    Vector3 startScale;
    public bool horizontalFace = true;
    Animator anim;
    Vector3 startPos;
    protected float distance;
    SpriteRenderer rend;

    //Enemy drops
    public float moneyDropChance;
    public GameObject moneyDrop;
    public float healthDropChance;
    public GameObject healthDrop;
    public float ammoDropChance;
    public GameObject[] ammoDrop;
    public float allAmmoDropChance;
    public GameObject allAmmoDrop;

    [Range(1, 250)]
    public float spawnScore;

    public SpriteRenderer bloodRend;
    public Sprite[] bloodSprites;

    public ParticleSystem dmgParts;
    public int dmgPartsAmt = 5;

    //Dead Zombie sprite
    public Sprite deadSprite;

    AudioSource src;
    public float lowPitch;
    public float highPitch;

    private void Awake()
    {
        cont = FindObjectOfType<GameController>();
        src = GetComponent<AudioSource>();
        startPos = transform.position;
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        startScale = transform.localScale;
        seeker = GetComponent<Seeker>();
        bod = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    public virtual void OnEnable()
    {
        curSpd = spd;
        Invoke("FindPlayer", 0.1f);
    }

    protected void FindPlayer()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    protected void CheckHpAnim()
    {
        if (hp < (maxHp / 3))
        {
            bloodRend.sprite = bloodSprites[2];
            Debug.Log("hurting bad");
        }
        else if (hp < (2 * maxHp / 3))
        {
            bloodRend.sprite = bloodSprites[1];
            Debug.Log("hurt");
        }
        else
        {
            bloodRend.sprite = bloodSprites[0];
            Debug.Log("fine");
        }
    }

    public virtual void Damage(float damage) 
    {
        src.pitch = Random.Range(lowPitch, highPitch);
        src.Play();
    }
    public virtual void Die()
    {
        cont.FreezeTime();
    }

    void UpdatePath()
    {
        if (target == null)
        {
            seeker.StartPath(bod.position, startPos, OnPathComplete);
            return;
        }

        if (seeker.IsDone() && target != null) seeker.StartPath(bod.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
        else Debug.Log("Error making path");
    }

    public void MovePath()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - bod.position).normalized;
        Vector2 force = direction * curSpd * Time.deltaTime;

        bod.AddForce(force);

        distance = Vector2.Distance(bod.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        /*
        //Face the direction we're moving
        if (horizontalFace)
        {
            if (bod.velocity.x >= 0.01f)
            {
                //transform.localScale = startScale;
                rend.flipX = false;
            }
            else if (bod.velocity.x <= -0.01f)
            {
                //transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
                rend.flipX = true;
            }
        }
        else
        {
            if (bod.velocity.y >= 0.01f)
            {
                //Face up
                anim.SetInteger("dir", 1);
            }
            else if (bod.velocity.y <= -0.01f)
            {
                //Face down
                anim.SetInteger("dir", 0);
            }
        }*/
        anim.SetFloat("moveX", bod.velocity.x);
        anim.SetFloat("moveY", bod.velocity.y);
    }

    public virtual void Idle() { }

    public virtual void Chase() { }

    public virtual void Attack() { }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Wire"))
        {
            curSpd = slowSpd;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wire"))
        {
            curSpd = spd;
        }
    }
}
