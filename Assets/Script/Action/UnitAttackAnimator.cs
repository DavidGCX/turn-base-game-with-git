using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackAnimator : MonoBehaviour
{
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform Gunfront;
    [SerializeField] private Transform Gunback;

    private void Start() {
        if(TryGetComponent<ShootAction>(out ShootAction shootAction)) {
            shootAction.OnShoot += ShootAction_OnShootEvent;
        }
    }

    private void ShootAction_OnShootEvent(object sender, ShootAction.ShootEventArgs e)
    {
        Transform bullet = Instantiate(bulletPrefab, Gunfront.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetUp((AttackAction)sender, e.shootDirection, e.TargetPosition);
    }
}
