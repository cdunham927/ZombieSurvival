using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbedWireEquipment : Equipment 
{
    public GameObject barbedWire;

    public override void UseItem()
    {
        base.UseItem();
        PlayerController pl = FindObjectOfType<PlayerController>();

        GameObject b = Instantiate(barbedWire, pl.transform.position, Quaternion.identity);
    }
}
