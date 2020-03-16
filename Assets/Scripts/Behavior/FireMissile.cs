using UnityEngine.Assertions;
using UnityEngine;

public class FireMissile : MonoBehaviour
{
    [Tooltip("in seconds")]
    public float fireRate = 3;

    private GameObject rocket = null;
    private Vector3 forward;

    private void Start()
    {
        rocket = Resources.Load<GameObject>("Prefabs/Missile");
        Assert.IsNotNull(rocket);

        // sprite is facing backwards so this is the only way.
        forward = transform.forward * -1;
        Debug.Log(forward);
        Debug.Log(transform.rotation);
        Debug.Log(transform.localScale);

        InvokeRepeating("Fire", 0.05f, fireRate);
    }

    private void Fire()
    {
        GameObject temp = Instantiate(rocket);
        temp.transform.position = transform.position;
        temp.transform.forward = forward;
        temp.transform.localScale = new Vector3(-1, 1, 1);
    }
}
