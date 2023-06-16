using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public string targetTag = "Player";
    public float atk = 1;
    //Damage
    float iframes;
    [Range(0, 1f)]
    public float iframeTime = 0.25f;

    private void Update()
    {
        if (iframes > 0) iframes -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag) && iframes <= 0)
        {
            collision.GetComponent<IDamageable<float>>().Damage(atk);
            iframes = iframeTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag) && iframes <= 0)
        {
            collision.GetComponent<IDamageable<float>>().Damage(atk);
            iframes = iframeTime;
        }
    }
}
