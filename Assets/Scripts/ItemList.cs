using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public List<Items> weaponList;
    WeaponController weapons;
    PlayerController player;
    public int numWeap = 0;

    private void Awake()
    {
        weapons = FindObjectOfType<WeaponController>();
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                BuyItem(weaponList[numWeap]);
                numWeap++;
            }
        }
    }

    public void BuyItem(Items item)
    {
        if (player.money >= item.cost && weapons.hasRoom())
        {
            player.money -= item.cost;
            weapons.AddItem(item);
        }
    }
}