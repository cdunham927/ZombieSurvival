using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explodeObj;
    Rigidbody2D bod;
    public float pushForce;

    //public GameObject fireObj;
    public Animator anim;
    public AnimationClip explodeClip;
    public bool hasExploded;

    public ScriptableFloat dmg;

    //public float explodeRadius;
    //public LayerMask enemyMask;

    GameController cont;
    public AudioClip clip;
    [Range(0, 1)]
    public float vol;

    //Disable after going far enough/hitting something
    public float disableTime;
    public float explodeVelocity;

    public float rotSpd;
    int dir;

    bool canDisable = false;

    private void OnEnable()
    {
        canDisable = false;
        Invoke("CanDisable", 0.125f);
        int dir = (Random.value > 0.5f) ? 1 : -1;
        hasExploded = false;
        Invoke("Disable", disableTime);
    }

    void CanDisable()
    {
        canDisable = true;
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void Disable()
    {
        Explode();
    }

    private void Awake()
    {
        cont = FindObjectOfType<GameController>();
        anim = GetComponent<Animator>();
        bod = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (dir == 1) transform.Rotate(0, 0, rotSpd * Time.deltaTime);
        else transform.Rotate(0, 0, -rotSpd * Time.deltaTime);
        if (bod.velocity.magnitude <= explodeVelocity && canDisable) Explode();
    }

    public void Push()
    {
        bod.AddForce(transform.up * pushForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Enemy") || collision.CompareTag("Wall"))  && !hasExploded)
        {
            Explode();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.CompareTag("Enemy") || collision.CompareTag("Wall")) && !hasExploded)
        {
            Explode();
        }
    }

    void Explode()
    {
        hasExploded = true;
        //anim.SetTrigger("Explode");
        //cont.PlaySound(clip, vol);
        //Instantiate(fireObj, transform.position, Quaternion.identity);
        //gameObject.SetActive(false);

        //Do explosion check here
        //Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explodeRadius, enemyMask);
        //
        //foreach (Collider2D c in cols)
        //{
        //    c.GetComponent<EnemyController>().Damage(dmg.val);
        //}
        Instantiate(explodeObj, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        //Invoke("Disable", 0.001f);
    }
}
