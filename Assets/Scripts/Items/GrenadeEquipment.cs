using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeEquipment : Equipment
{
    public GameObject grenadePrefab;

    public override void UseItem()
    {
        //base.UseItem();
        PlayerController pl = FindObjectOfType<PlayerController>();

        Grenade g = Instantiate(grenadePrefab, pl.transform.position, Quaternion.identity).GetComponent<Grenade>();
        weapons = pl.GetComponentInChildren<WeaponController>();
        g.transform.rotation = weapons.transform.rotation * Quaternion.Euler(0, 0, -90);
        g.Push();
    }
}
