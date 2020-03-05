using UnityEngine.Assertions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float LowestY { private get; set; }
    private Vector3 restartPosition;

    private Rigidbody2D rb = null;

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
        Debug.Log(LowestY);
        if (transform.position.y < LowestY)
        {
            Debug.LogWarning("Nothing else is done with death at the moment.");
            rb.velocity = Vector2.zero;
            transform.position = restartPosition;
        }
    }
}
