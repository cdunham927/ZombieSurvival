using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : MonoBehaviour
{
    Rigidbody2D bod;
    public float pushForce;

    public GameObject fireObj;

    private void Awake()
    {
        bod = GetComponent<Rigidbody2D>();
    }

    public void Push()
    {
        bod.AddForce(transform.up * pushForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Wall"))
        {
            Explode();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Wall"))
        {
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(fireObj, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
