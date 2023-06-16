using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public ScriptableFloat distanceToPickup;
    public ScriptableFloat spd;
    public ScriptableFloat timeToDisable;
    float curTime;

    protected PlayerController player;
    protected WeaponController weapon;

    public bool canPickup = true;

    bool hasPickedUp;

    protected AudioSource src;
    public AudioClip pickupClip;
    [Range(0, 1)]
    public float sndVolume;

    private void Awake()
    {
        src = FindObjectOfType<GameController>().soundSrc;
        player = FindObjectOfType<PlayerController>();
        weapon = FindObjectOfType<WeaponController>();
    }

    protected virtual void OnEnable()
    {
        curTime = timeToDisable.val;
        hasPickedUp = false;
    }

    protected virtual void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance <= distanceToPickup.val && canPickup)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, spd.val * Time.deltaTime);
            }
        }

        if (curTime > 0)
        {
            curTime -= Time.deltaTime;
        }
        else Invoke("Disable", 0.01f);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    public virtual void GetPickup() 
    {
        if (src != null)
        {
            src.volume = sndVolume;
            src.PlayOneShot(pickupClip);
        }
        hasPickedUp = true;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canPickup && !hasPickedUp)
        {
            GetPickup();
        }
    }
}
