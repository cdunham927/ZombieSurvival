using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public float spd;
    Rigidbody2D bod;
    //GameController cont;
    Vector3 startSize;
    WeaponController weapon;
    public float life = 2f;
    public float knockback = 0f;

    GameController cont;
    public AudioClip clip;
    public float vol;

    private void Awake()
    {
        cont = FindObjectOfType<GameController>();
        weapon = FindObjectOfType<WeaponController>();
        //cont = FindObjectOfType<GameController>();
        bod = GetComponent<Rigidbody2D>();
        //startSize = transform.localScale;
    }

    private void OnEnable()
    {
        Invoke("Disable", life);
        bod.AddForce(transform.up * spd);
        //transform.localScale = startSize;
    }

    /*private void Update()
    {
        if (transform.localScale.magnitude < 0.01f)
        {
            gameObject.SetActive(false);
        }
    }*/

    private void OnDisable()
    {
        CancelInvoke();
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //collision.GetComponent<EnemyController>().Damage(atk);
            cont.ActivateExplosion(transform.position);
            cont.PlaySound(clip, vol);
            collision.attachedRigidbody.AddForce(transform.up * knockback);
            Invoke("Disable", 0.001f);
        }
        if (collision.CompareTag("Wall"))
        {
            cont.PlaySound(clip, vol);
            cont.ActivateExplosion(transform.position);
            Invoke("Disable", 0.001f);
        }
    }
}
