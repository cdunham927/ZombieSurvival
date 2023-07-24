using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSquidController : EnemyController, IDamageable<float>, IKillable
{
    Animator anim;
    Vector3 startPos;
    public GameObject bullet;
    float cools = 0f;
    float distanceToTarget;

    public override void OnEnable()
    {
        base.OnEnable();

        startPos = transform.position;
        hp = maxHp;
    }

    void FixedUpdate()
    {
        distanceToTarget = Vector2.Distance(bod.position, target.position);

        if (distanceToTarget <= attackRange && cools <= 0) Attack();

        MovePath();

        if (cools > 0)
        {
            cools -= Time.deltaTime;
        }

        //Debug.Log(Vector3.Distance(transform.position, PlayerController.player.transform.position));

        health.fillAmount = Mathf.Lerp(health.fillAmount, (hp / maxHp), lerpSpd * Time.deltaTime);
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);

        hp -= damage;

        //anim.Play("ZombieSquidHurt");
        //Invoke("ResetHurt", 0.001f);
        //Debug.Log("Enemy hp: " + hp);

        if (hp <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        base.Die();

        Instantiate(deadSquid, transform.position, Quaternion.identity);
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

    public override void Attack()
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + Random.Range(-15f, 15f) - 90f;
        Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        cools = timeBetweenAttacks;
        curState = states.chase;
    }
}