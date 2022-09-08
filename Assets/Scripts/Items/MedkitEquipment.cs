using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitEquipment : Equipment
{
    //If we have a healing effect or particle system, we can activate it when we use the item
    //
    //
    //


    //public GameObject barbedWire;
    public float amt;

    public override void UseItem()
    {
        PlayerController pl = FindObjectOfType<PlayerController>();

        pl.Heal(amt);
    }
}
