using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doctor : PlayerController
{
    //Overheals the player to double their max hp
    //Buffs the player for a short time
    //Hp reverts to normal over time
    public float hpRevertLerp;

    public override void UseSpecial()
    {
        hpImg.color = Color.yellow;
        curHp = overhealMaxHp;
        curSpecialCooldown = specialCooldown;
    }

    public override void AddMoney(float amt)
    {
        base.AddMoney(amt);
    }

    public override void Heal(float amt)
    {
        base.Heal(amt);
    }

    public override void Update()
    {
        base.Update();

        if (curHp > maxHp)
        {
            curHp -= Time.deltaTime * hpRevertLerp;
        }
    }
}
