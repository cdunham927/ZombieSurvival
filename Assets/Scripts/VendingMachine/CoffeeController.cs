using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeController : VMItem
{
    public override void Effect()
    {
        PlayerController player;
        player = FindObjectOfType<PlayerController>();
        player.money -= price;
        player.SetCoffee();

        base.Effect();
    }
}
