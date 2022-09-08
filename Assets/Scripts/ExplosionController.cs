using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public AnimationClip explodeClip;
    public float dmg;
    public float fireTime = 3f;

    //Temp until we get raycast damage working
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>().Damage(dmg);
        }
    }

    //Temp until we get raycast damage working
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>().Damage(dmg);
        }
    }

    private void OnEnable()
    {
        //Need to do raycast here to check for who gets damaged

        Invoke("Disable", explodeClip.length);
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
