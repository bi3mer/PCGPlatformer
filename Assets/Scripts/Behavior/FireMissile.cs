using UnityEngine.Assertions;
using UnityEngine;
using System;

public class FireMissile : MonoBehaviour
{
    [Tooltip("in seconds")]
    public float fireRate = 3;

    public Action HitPlayerCallback = null;

    private GameObject rocket = null;
    private Vector3 forward;

    private void Start()
    {
        rocket = Resources.Load<GameObject>("Prefabs/Missile");
        Assert.IsNotNull(rocket);

        // sprite is facing backwards so this is the only way.
        forward = transform.forward * -1;

        InvokeRepeating("Fire", 0.05f, fireRate);
    }

    private void Fire()
    {
        GameObject temp = Instantiate(rocket);
        temp.GetComponent<AttackPlayer>().AddHitEnemyCallback(HitPlayerCallback);
        temp.transform.position = transform.position;
        temp.transform.forward = forward;
        temp.transform.localScale = new Vector3(-1, 1, 1);
    }
}
