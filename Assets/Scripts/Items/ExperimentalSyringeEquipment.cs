using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentalSyringeEquipment : Equipment
{
    public GameObject syringe;

    public override void UseItem()
    {
        base.UseItem();
        PlayerController pl = FindObjectOfType<PlayerController>();

        ExperimentalSyringe s = Instantiate(syringe, pl.transform.position, Quaternion.identity).GetComponent<ExperimentalSyringe>();
        weapons = pl.GetComponentInChildren<WeaponController>();
        s.transform.rotation = weapons.transform.rotation * Quaternion.Euler(0, 0, 0);
        s.Push();
    }
}
