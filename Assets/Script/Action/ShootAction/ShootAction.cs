using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ShootAction : AttackAction
{

    // unit in baseAction is inherited through selfUnit here. Access it through selfUnit.
    public event EventHandler<ShootEventArgs> OnShoot;

    public class ShootEventArgs : EventArgs {
        public Vector3 shootDirection;
        public Vector3 TargetPosition;
    }
    protected override IEnumerator SpecificAttack()
    {
        Debug.Log(LevelGrid.instance.GetGridDistance(targetUnit.GetWorldPosition(), unit.GetWorldPosition()) + " GridDitance away");
        OnShoot?.Invoke(this, new ShootEventArgs {
            shootDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized
            ,
            TargetPosition = targetUnit.GetWorldPosition(),
        });
        animator.Play("firing rifle");
        yield return new WaitForSeconds(1f);
    }

    protected override bool ShouldUseAttackCamera()
    {
        if (GetTotalDamage() >= targetUnit.GetUnitCurrentHealth()) {
            return true;
        } else {
            return false;
        }
    }

    protected override int CalculateEnemyAIActionValue() => 100;
}
