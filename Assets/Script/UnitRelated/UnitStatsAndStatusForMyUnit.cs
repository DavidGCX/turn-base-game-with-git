using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatsAndStatusForMyUnit : UnitStatsAndStatusBase
{
    public override bool Damage(int baseDamage, int apDamage, int totalAttack, float damageRandomRate)
    {
        return base.Damage(baseDamage, apDamage, totalAttack, damageRandomRate);
    }

    public override void NewTurn()
    {
        base.NewTurn();
    }

    protected override int CalculateBaseDamageAfterArmor(int baseDamage)
    {
        return base.CalculateBaseDamageAfterArmor(baseDamage);
    }

    protected override void Dead()
    {
        base.Dead();
    }
}
