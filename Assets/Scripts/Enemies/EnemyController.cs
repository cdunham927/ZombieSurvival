using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public abstract class EnemyController : MonoBehaviour
{
    public float hp = 50;
    public float maxHp = 50;
    public GameObject deadSquid;
    public float chaseDistance;
    public float attackRange;
    public Image health;
    public float lerpSpd = 5f;
    public float timeBetweenAttacks;

    //AI stuff
    public Transform target;
    public float nextWaypointDistance = 2f;
    public float spd;
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

    private void Awake()
    {
        startPos = transform.position;
        anim = GetComponent<Animator>();
        startScale = transform.localScale;
        seeker = GetComponent<Seeker>();
        bod = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        target = FindObjectOfType<PlayerController>().transform;
        if (target == null)
        {
            seeker.StartPath(bod.position, startPos, OnPathComplete);
            return;
        }

        if (seeker.IsDone()) seeker.StartPath(bod.position, target.position, OnPathComplete);
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
        Vector2 force = direction * spd * Time.deltaTime;

        bod.AddForce(force);

        distance = Vector2.Distance(bod.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        //Face the direction we're moving
        if (horizontalFace)
        {
            if (bod.velocity.x >= 0.01f)
            {
                transform.localScale = startScale;
            }
            else if (bod.velocity.x <= -0.01f)
            {
                transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
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
        }
    }

    public virtual void Idle() { }

    public virtual void Chase() { }

    public virtual void Attack() { }
}
