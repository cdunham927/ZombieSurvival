using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSquidProjectileController : MonoBehaviour
{
    public float atkLow;
    public float atkHigh;
    public float atk;
    public float spd;
    Rigidbody2D bod;
    //GameController cont;
    Vector3 startSize;

    private void Awake()
    {
        //cont = FindObjectOfType<GameController>();
        bod = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        atk = Mathf.RoundToInt(Random.Range(atkLow, atkHigh));
        Invoke("Disable", 2f);
        bod.AddForce(transform.up * spd);
    }

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
        if (collision.CompareTag("Player"))
        {
            PlayerController.player.Damage(atk);
            Invoke("Disable", 0.001f);
        }
    }
}
