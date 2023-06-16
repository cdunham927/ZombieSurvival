using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovEquipment : Equipment
{
    public GameObject molotovPrefab;

    public override void UseItem()
    {
        base.UseItem();
        PlayerController pl = FindObjectOfType<PlayerController>();

        Molotov m = Instantiate(molotovPrefab, pl.transform.position, Quaternion.identity).GetComponent<Molotov>();
        weapons = pl.GetComponentInChildren<WeaponController>();
        m.transform.rotation = weapons.transform.rotation * Quaternion.Euler(0, 0, -90);
        m.Push();
    }
}
