using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ShootAction : AttackAction
{
    [Header("子弹的prefab")]
    [SerializeField] private Transform bulletPrefab;
    [Header("子弹生成的位置")]
    [SerializeField] private Transform gunFront;

    [SerializeField] private Transform gunBack;
    protected override IEnumerator SpecificAttack()
    {
        animator.Play("firing rifle");
        Transform bullet = Instantiate(bulletPrefab, gunFront.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetUp(this, (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized, targetUnit.GetWorldPosition());
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
