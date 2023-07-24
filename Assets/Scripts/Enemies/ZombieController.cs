using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieController : EnemyController
{
    Animator anim;
    //public GameObject bullet;
    float cools = 0f;

    public override void OnEnable()
    {
        base.OnEnable();

        hp = maxHp;
        int animToPick = Random.Range(0, 2);
    }

    
    void FixedUpdate() 
    {
        MovePath();

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

        //anim.SetInteger("curState", (int)curState);

        if (cools > 0)
        {
            cools -= Time.deltaTime;
        }

        //Debug.Log(Vector3.Distance(transform.position, PlayerController.player.transform.position));

        health.fillAmount = Mathf.Lerp(health.fillAmount, (hp / maxHp), lerpSpd * Time.deltaTime);

        //Debug.DrawLine(transform.position, transform.position * Vector2.right * attackRange);
    }

    void Update()
    {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.N))
        {
            Damage(999);
        }
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);

        hp -= damage;

        //anim.Play("ZombieHurt");

        //Invoke("ResetHurt", 0.001f);
        //Debug.Log("Enemy hp: " + hp);

        if (hp <= 0)
        {
            Die();
        }

        dmgParts.Emit(dmgPartsAmt);
        CheckHpAnim();
    }

    public override void Die()
    {
        base.Die();

        //If we roll good enough, drop an item for the player
        float chance = Random.value;

        if (chance < moneyDropChance)
        {
            Instantiate(moneyDrop, transform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f)), Quaternion.identity);
        }
        else if (chance < healthDropChance)
        {
            Instantiate(healthDrop, transform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f)), Quaternion.identity);
        }
        else if (chance < allAmmoDropChance)
        {
            Instantiate(allAmmoDrop, transform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f)), Quaternion.identity);
        }
        else if (chance < ammoDropChance)
        {
            //Instantiate(ammoDrop[Random.Range(0, ammoDrop.Length)], transform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f)), Quaternion.identity);
        }

        BodyController body = Instantiate(deadSquid, transform.position, Quaternion.identity).GetComponent<BodyController>();
        body.ChangeSprite(deadSprite);

        //Object pool this later
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
        if (distance <= chaseDistance)
        {
            curState = states.chase;
        }
    }

    public override void Chase()
    {
        if (distance > chaseDistance)
        {
            curState = states.idle;
        }
        if (distance < attackRange && cools <= 0)
        {
            curState = states.attack;
        }
    }

    public override void Attack()
    {
        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + Random.Range(-15f, 15f) - 90f;
            Quaternion.AngleAxis(angle, Vector3.forward);
            //Instantiate(bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            cools = timeBetweenAttacks;
            curState = states.chase;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
