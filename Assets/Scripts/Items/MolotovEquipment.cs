using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovEquipment : Equipment
{
    public GameObject molotovPrefab;

    public override void UseItem()
    {
        PlayerController pl = FindObjectOfType<PlayerController>();

        Molotov m = Instantiate(molotovPrefab, pl.transform.position, Quaternion.identity).GetComponent<Molotov>();
        m.transform.rotation = weapons.transform.rotation;
        m.Push();
    }
}
