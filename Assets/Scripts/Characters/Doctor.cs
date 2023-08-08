using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doctor : PlayerController
{

    //Overheals the player to double their max hp
    //Buffs the player for a short time
    //Hp reverts to normal over time
    public override void UseSpecial()
    {
        hpImg.color = Color.yellow;
        curHp = overhealMaxHp;
        specialCooldown = doctorSpecialCooldown;
    }
}
