using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillAllAmmoPickup : PickupController
{    protected override void Update()
    {
        canPickup = weapon.needsAnyAmmo();

        base.Update();
    }

    public override void GetPickup()
    {
        //Add ammo to player
        weapon.FillAmmo();

        base.GetPickup();
    }
}
