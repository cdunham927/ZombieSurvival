using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    Rigidbody2D bod;
    public float pushForce;

    //public GameObject fireObj;
    public Animator anim;
    public AnimationClip explodeClip;
    public bool hasExploded;

    public ScriptableFloat dmg;

    public float explodeRadius;
    public LayerMask enemyMask;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        bod = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        hasExploded = false;
    }

    public void Push()
    {
        bod.AddForce(transform.up * pushForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Enemy") || collision.CompareTag("Wall"))  && !hasExploded)
        {
            hasExploded = true;
            Invoke("Disable", explodeClip.length);
            anim.SetTrigger("Explode");
            Explode();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.CompareTag("Enemy") || collision.CompareTag("Wall")) && !hasExploded)
        {
            hasExploded = true;
            Invoke("Disable", explodeClip.length);
            anim.SetTrigger("Explode");
            Explode();
        }
    }

    void Explode()
    {
        //Instantiate(fireObj, transform.position, Quaternion.identity);
        //gameObject.SetActive(false);

        //Do explosion check here
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explodeRadius, enemyMask);

        foreach (Collider2D c in cols)
        {
            c.GetComponent<EnemyController>().Damage(dmg.val);
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
