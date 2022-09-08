using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : PickupController
{
    public float amtToAdd = 50f;

    protected override void Update()
    {
        canPickup = (player.GetHealth() < player.maxHp);

        base.Update();
    }

    public override void GetPickup()
    {
        player.Heal(amtToAdd);

        base.GetPickup();
    }
}
