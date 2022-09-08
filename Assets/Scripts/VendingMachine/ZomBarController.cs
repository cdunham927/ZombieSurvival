using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomBarController : VMItem
{
    public override void Effect()
    {
        PlayerController player;
        player = FindObjectOfType<PlayerController>();
        WeaponController weapon;
        weapon = FindObjectOfType<WeaponController>();
        player.money -= price;
        weapon.SetZombar();

        base.Effect();
    }
}
