using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float atkLow;
    public float atkHigh;
    public float atk;
    public float spd;
    Rigidbody2D bod;
    //GameController cont;
    Vector3 startSize;
    WeaponController weapon;
    public float life = 2f;
    public bool piercing = false;
    public float knockback = 0f;

    private void Awake()
    {
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

    public void SetDamage(float low, float high)
    {
        atk = Mathf.RoundToInt(Random.Range(low, high));
    }

    void Disable()
    {
        //transform.localScale *= 20f * Time.deltaTime;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        { 
            collision.GetComponent<IDamageable<float>>().Damage(atk);
            collision.attachedRigidbody.AddForce(transform.up * knockback);
            if (!piercing)
            {
                Invoke("Disable", 0.001f);
            } else
            {
                //Debug.Log("Did " + atk + " damage");
                atk /= 2f;
                //Debug.Log("Now does " + atk + " damage");
            }
        }
    }
}
