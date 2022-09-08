using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : VMItem
{
    public float amt;

    public override void Effect()
    {
        PlayerController player;
        player = FindObjectOfType<PlayerController>();
        player.money -= price;
        player.Heal(amt);

        base.Effect();
    }
}
