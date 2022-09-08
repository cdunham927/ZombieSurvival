using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : PickupController
{
    public Items.types ammoType;

    protected override void Update()
    {
        canPickup = weapon.needsAmmo(ammoType);

        base.Update();
    }

    public override void GetPickup()
    {
        //Add ammo to player
        weapon.RefillAmmo(ammoType);

        base.GetPickup();
    }
}
