using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineEquipment : Equipment
{
    public GameObject mine;

    public override void UseItem()
    {
        base.UseItem();
        PlayerController pl = FindObjectOfType<PlayerController>();
        GameObject b = Instantiate(mine, pl.transform.position, Quaternion.identity);
    }
}
