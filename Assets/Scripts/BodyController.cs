using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public float life = 10f;
    float lifeLeft = 0f;
    SpriteRenderer rend;
    public ParticleSystem parts;
    public int partsToEmit;

    public float rotationRangeHigh = -75f;
    public float rotationRangeLow = -90f;
    float rotationRange;

    public SpriteRenderer bloodPoolRend;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public void ChangeSprite(Sprite newSprite)
    {
        //Change sprite to match zombie
        rend.sprite = newSprite;
    }

    private void OnEnable()
    {
        //Adjust start rotation
        rotationRange = Random.Range(rotationRangeLow, rotationRangeHigh);
        transform.rotation = Quaternion.Euler(0, 0, rotationRange);

        //Throw particles
        parts.Emit(partsToEmit);
        //Disable after certain amount of time
        lifeLeft = life;
        rend.flipY = (Random.value > 0.5f) ? true : false;
        //if (rend.flipY)
        //{
        //    bloodPoolRend.flipX = true;
        //    bloodPoolRend.flipY = true;
        //}
        //else
        //{
        //    bloodPoolRend.flipX = false;
        //    bloodPoolRend.flipY = false;
        //}
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
