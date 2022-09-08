using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickup : PickupController
{
    public float amtToAdd = 50f;

    protected override void OnEnable()
    {
        canPickup = true;

        base.OnEnable();
    }

    public override void GetPickup()
    {
        player.AddMoney(amtToAdd);

        base.GetPickup();
    }
}
