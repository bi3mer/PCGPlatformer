using UnityEngine.Assertions;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float LowestY { private get; set; }
    private Vector3 restartPosition;

    private Rigidbody2D rb = null;

    public Action PlayerDiedCallback = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb);
    }

    private void Start()
    {
        restartPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (transform.position.y < LowestY)
        {
            if (PlayerDiedCallback == null)
            {
                rb.velocity = Vector2.zero;
                transform.position = restartPosition;
            }
            else
            {
                PlayerDiedCallback.Invoke();
            }
        }
    }
}
