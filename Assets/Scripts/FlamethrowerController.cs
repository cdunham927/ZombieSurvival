using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerController : MonoBehaviour
{
    public float damage = 1;
    ParticleSystem parts;

    private void Awake()
    {
        parts = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        parts.Emit(2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent < IDamageable<float>>().Damage(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent < IDamageable<float>>().Damage(damage);
        }
    }
}
