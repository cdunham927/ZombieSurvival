using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Equipment : Buyable
{
    [Header("In game things")]
    public float sinAmplitude;
    public float sinFrequency;
    protected Vector3 pos;

    //For picking up
    protected bool canPickup = false;
    protected Animator anim;

    //For buying
    public TextMeshProUGUI buyText;
    protected WeaponController weapons;

    public float refillCost;
    public int fills;

    private void Awake()
    {
        weapons = FindObjectOfType<WeaponController>();
        buyText = GetComponentInChildren<TextMeshProUGUI>();
        pos = transform.position;
        anim = GetComponent<Animator>();
        sinAmplitude = Random.Range(0.075f, 0.125f);
        sinFrequency = Random.Range(0.75f, 1.25f);
    }

    public virtual void UseItem() { }

    void Update()
    {
        //move it up and down ~
        float sinY = sinAmplitude * Mathf.Sin(Time.time * sinFrequency);
        transform.position = new Vector3(pos.x, pos.y + sinY);

        if (canPickup)
        {
            //Player doesnt have the weapon yet
            //show buytext
            if (!weapons.hasEquipment(this))
            {
                if (PlayerController.player.money > cost) buyText.text = "<color=white>" + itemName + " - </color>" + "<color=yellow> $" + cost.ToString() + "</color>\n";
                else buyText.text = "<color=white>" + itemName + " - </color>" + "<color=red> $" + cost.ToString() + "</color>\n";
            }
            //Player has weapon, they can buy ammo
            //show buytext
            else
            {
                if (PlayerController.player.money > refillCost) buyText.text = "<color=white>" + itemName + " ammo - </color>" + "<color=yellow> $" + cost.ToString() + "</color>\n";
                else buyText.text = "<color=white>" + itemName + " ammo - </color>" + "<color=red> $" + refillCost.ToString() + "</color>\n";
            }

            if (PlayerController.player.money > cost && Input.GetKeyDown(KeyCode.E))
            {
                //Buy weapon
                if (!weapons.hasEquipment(this))
                {
                    PlayerController.player.money -= cost;
                    weapons.SwitchEquipment(this);
                    weapons.RefillEquip();
                    //gameObject.SetActive(false);
                }
                //Buy ammo
                else
                {
                    PlayerController.player.money -= refillCost;
                    weapons.RefillEquip();
                    //gameObject.SetActive(false);
                }
            }
        }

        anim.SetBool("canPickup", canPickup);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = false;
        }
    }
}
