using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeEquipment : Equipment
{
    public GameObject grenadePrefab;

    public override void UseItem()
    {
        PlayerController pl = FindObjectOfType<PlayerController>();

        Grenade g = Instantiate(grenadePrefab, pl.transform.position, Quaternion.identity).GetComponent<Grenade>();
        g.transform.rotation = weapons.transform.rotation;
        g.Push();
    }
}
