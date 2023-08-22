using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentalSyringe : MonoBehaviour
{
    Rigidbody2D bod;
    public float pushForce;

    public ScriptableFloat healAmt;

    GameController cont;
    public AudioClip clip;
    [Range(0, 1)]
    public float vol;

    //Disable after going far enough/hitting something
    public float disableTime;

    bool canDisable = false;

    private void OnEnable()
    {
        canDisable = false;
        Invoke("CanDisable", 0.125f);
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
        Destroy();
    }

    private void Awake()
    {
        cont = FindObjectOfType<GameController>();
        bod = GetComponent<Rigidbody2D>();
    }

    public void Push()
    {
        bod.AddForce(transform.up * pushForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && canDisable)
        {
            collision.GetComponent<EnemyController>().Convert();
            Destroy();
        }
        if (collision.CompareTag("NPC") && canDisable)
        {
            collision.GetComponent<NPCController>().Heal(healAmt.val);
            Destroy();
        }
        if (collision.CompareTag("Wall") && canDisable)
        {
            Destroy();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && canDisable)
        {
            collision.GetComponent<EnemyController>().Convert();
            Destroy();
        }
        if (collision.CompareTag("NPC") && canDisable)
        {
            collision.GetComponent<NPCController>().Heal(healAmt.val);
            Destroy();
        }
        if (collision.CompareTag("Wall") && canDisable)
        {
            Destroy();
        }
    }

    void Destroy()
    {
        canDisable = false;
        gameObject.SetActive(false);
    }
}
