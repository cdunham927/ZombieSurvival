using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    //public GameObject fireObj;
    public Animator anim;
    public bool hasExploded;

    public ScriptableFloat dmg;

    GameController cont;
    public AudioClip clip;
    [Range(0, 1)]
    public float vol;

    private void Awake()
    {
        cont = FindObjectOfType<GameController>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        hasExploded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Enemy")) && !hasExploded)
        {
            hasExploded = true;
            cont.ActivateExplosion(transform.position);
            cont.PlaySound(clip, vol);
            Invoke("Disable", 0.001f);
            //anim.SetTrigger("Explode");
            //Explode();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.CompareTag("Enemy")) && !hasExploded)
        {
            hasExploded = true;
            cont.ActivateExplosion(transform.position);
            cont.PlaySound(clip, vol);
            Invoke("Disable", 0.001f);
            //anim.SetTrigger("Explode");
            //Explode();
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
