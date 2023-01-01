using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform front;
    [SerializeField] private Transform back;
    float speed = 10000f;

    public void fire() {
        Transform bullet = Instantiate(bulletPrefab, front.position, Quaternion.identity);
        Vector3 moveDirection = (front.position - back.position).normalized;
        bullet.GetComponent<Rigidbody>().AddForce(moveDirection * speed);
    }
}
