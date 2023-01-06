using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ShootAction : AttackAction
{
    public event EventHandler<ShootEventArgs> OnShoot;

    public class ShootEventArgs : EventArgs {
        public Vector3 shootDirection;
        public Vector3 TargetPosition;
    }
    protected override IEnumerator SpecificAttack()
    {
        OnShoot?.Invoke(this, new ShootEventArgs {
            shootDirection = (TargetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized
            ,
            TargetPosition = TargetUnit.GetWorldPosition()
        });
        animator.Play("firing rifle");
        yield return new WaitForSeconds(1f);
        
    }
}
