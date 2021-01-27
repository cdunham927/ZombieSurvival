using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public float life = 10f;
    float lifeLeft = 0f;
    SpriteRenderer rend;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        lifeLeft = life;
        rend.flipX = (Random.value > 0.5f) ? true : false;
    }

    private void Update()
    {
        lifeLeft -= Time.deltaTime;

        if (lifeLeft <= 0) Invoke("Disable", 0.01f);
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
