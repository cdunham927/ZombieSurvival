using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : VMItem
{
    public override void Effect()
    {
        PlayerController player;
        player = FindObjectOfType<PlayerController>();
        player.money -= price;
        player.SetMagnet();

        base.Effect();
    }
}
