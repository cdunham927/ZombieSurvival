using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Items : Buyable
{
    public enum types { pistol, shotgun, machinegun, sniper, rpg, flamethrower, sword, spear, axe, chakram, shuriken, healing, puzzlePiece }
    [Header("Weapon things")]
    public int id;
    public float dmgLow;
    public float dmgHigh;
    public types thisType;
    public float cooldown;
    public float spreadLow;
    public float spreadHigh;
    //Ammmo cost
    public float ammoCost;
    public float knockback;
    public List<Material> craftingMaterials;
    public int clipSize;
    public Sprite ammoImage;
    public float reloadTime;
    public bool singleReload = false;
    public int shellCount = 0;

    [Header("In game things")]
    public float sinAmplitude;
    public float sinFrequency;
    Vector3 pos;

    //For picking up
    bool canPickup = false;
    Animator anim;

    //For buying
    public TextMeshProUGUI buyText;
    WeaponController weapons;
    PlayerController player;

    public bool cocks = false;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        weapons = FindObjectOfType<WeaponController>();
        buyText = GetComponentInChildren<TextMeshProUGUI>();
        pos = transform.position;
        anim = GetComponent<Animator>();
        sinAmplitude = Random.Range(0.075f, 0.125f);
        sinFrequency = Random.Range(0.75f, 1.25f);
    }

    void Update()
    {
        //move it up and down ~
        float sinY = sinAmplitude * Mathf.Sin(Time.time * sinFrequency);
        transform.position = new Vector3(pos.x, pos.y + sinY);

        if (canPickup)
        {
            //Player doesnt have the weapon yet
            //show buytext
            if (!weapons.hasWeapon(this))
            {
                if (player.money > cost) buyText.text = "<color=white>" + itemName + " - </color>" + "<color=yellow> $" + cost.ToString() + "</color>\n";
                else buyText.text = "<color=white>" + itemName + " - </color>" + "<color=red> $" + cost.ToString() + "</color>\n";
            }
            //Player has weapon, they can buy ammo
            //show buytext
            else
            {
                if (player.money > ammoCost) buyText.text = "<color=white>" + itemName + " ammo - </color>" + "<color=yellow> $" + cost.ToString() + "</color>\n";
                else buyText.text = "<color=white>" + itemName + " ammo - </color>" + "<color=red> $" + ammoCost.ToString() + "</color>\n";
            }

            if (player.money > cost && Input.GetKeyDown(KeyCode.E))
            {
                //Buy weapon
                if (!weapons.hasWeapon(this))
                {
                    player.money -= cost;
                    weapons.AddItem(this);
                    //gameObject.SetActive(false);
                }
                //Buy ammo
                else
                {
                    player.money -= ammoCost;
                    weapons.RefillAmmo(thisType);
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