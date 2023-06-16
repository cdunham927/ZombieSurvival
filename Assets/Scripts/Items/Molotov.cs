using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : MonoBehaviour
{
    Rigidbody2D bod;
    public float pushForce;

    public GameObject fireObj;
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

    private void Update()
    {
        if (dir == 1) transform.Rotate(0, 0, rotSpd * Time.deltaTime);
        else transform.Rotate(0, 0, -rotSpd * Time.deltaTime);
        if (bod.velocity.magnitude <= explodeVelocity && canDisable) Explode();
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
        //cont.PlaySound(clip, vol);
        Instantiate(fireObj, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
