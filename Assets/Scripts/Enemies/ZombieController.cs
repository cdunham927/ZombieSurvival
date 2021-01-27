using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieController : EnemyController, IDamageable<float>, IKillable
{
    public float hp = 50;
    public float maxHp = 50;
    public GameObject deadSquid;
    Animator anim;
    Vector3 startPos;
    public float chaseDistance;
    public float attackRange;
    public float spd;
    //public GameObject bullet;
    float cools = 0f;
    public Image health;
    public float lerpSpd = 5f;
    public float timeBetweenAttacks;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        //health = GetComponentInChildren<Image>();
    }

    private void OnEnable()
    {
        startPos = transform.position;
        hp = maxHp;
        int animToPick = Random.Range(0, 2);
    }

    private void Update()
    {
        switch (curState)
        {
            case states.idle:
                Idle();
                break;
            case states.chase:
                Chase();
                break;
            case states.attack:
                Attack();
                break;
        }

        anim.SetInteger("curState", (int)curState);

        if (cools > 0)
        {
            cools -= Time.deltaTime;
        }

        if (PlayerController.player.transform.position.y > transform.position.y)
        {
            //anim.SetInteger("dir", 1);
        }
        else
        {
            //anim.SetInteger("dir", 0);
        }

        //Debug.Log(Vector3.Distance(transform.position, PlayerController.player.transform.position));

        health.fillAmount = Mathf.Lerp(health.fillAmount, (hp / maxHp), lerpSpd * Time.deltaTime);

        Debug.DrawLine(transform.position, transform.position * Vector2.right * attackRange);
    }

    public void Damage(float damage)
    {
        hp -= damage;

        anim.Play("ZombieSquidHurt");
        //Invoke("ResetHurt", 0.001f);
        //Debug.Log("Enemy hp: " + hp);

        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Instantiate(deadSquid, transform.position, Quaternion.identity);
        //Object pool this later
        //
        Invoke("Disable", 0.001f);
    }

    private void OnDisable()
    {
        //CancelInvoke();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    void ResetHurt()
    {
        anim.SetBool("hurt", false);
    }

    public override void Idle()
    {
        float distance = Vector3.Distance(transform.position, PlayerController.player.transform.position);
        if (distance <= chaseDistance)
        {
            curState = states.chase;
        }

        if (transform.position != startPos)
        {
            var step = spd * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, startPos, step);
        }
    }

    public override void Chase()
    {
        float distance = Vector3.Distance(transform.position, PlayerController.player.transform.position);
        if (distance > chaseDistance)
        {
            curState = states.idle;
        }
        if (distance < attackRange && cools <= 0)
        {
            curState = states.attack;
        }

        var step = spd * Time.deltaTime;

        if (distance > attackRange) transform.position = Vector3.MoveTowards(transform.position, PlayerController.player.transform.position, step);
    }

    public override void Attack()
    {
        Vector3 dir = PlayerController.player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + Random.Range(-15f, 15f) - 90f;
        Quaternion.AngleAxis(angle, Vector3.forward);
        //Instantiate(bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        cools = timeBetweenAttacks;
        curState = states.chase;
    }

    void OnDrawGizmoss()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
