using UnityEngine.Assertions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AttackPlayer))]
public class AccelerateBackAndForth : BaseBehavior
{
    [SerializeField]
    private float speed = 1f;

    private Vector3 velocity;
    private Rigidbody2D rb;

    private int Direction
    {
        get { return (int)transform.forward.z; }
    }

    private void Awake()
    {
        Assert.IsTrue(speed > 0);

        AttackPlayer attackPlayer = GetComponent<AttackPlayer>();
        Assert.IsNotNull(attackPlayer);

        attackPlayer.AddHitEnemyCallback(Collided);
        attackPlayer.AddHitPlayerCallback(Collided);

        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb);
        rb.velocity = new Vector3(speed, 0, 0);
    }

    private void FixedUpdate()
    {
        Vector3Int cellPos = Map.WorldToCell(transform.position);
        Vector3Int nextPos = new Vector3Int(cellPos.x + Direction, cellPos.y, cellPos.z);

        if (Map.GetTile(nextPos) != null)
        {
            Flip();
            rb.velocity = Vector2.zero;
        }
        else
        {
            Vector3Int belowNextPos = new Vector3Int(nextPos.x, nextPos.y - 1, nextPos.z);

            if (Map.GetTile(belowNextPos) == null)
            {
                Flip();
                rb.velocity = Vector2.zero;
            }
        }

        rb.velocity += new Vector2(speed * Direction * Time.fixedDeltaTime, 0);
    }

    private void Collided()
    {
        Flip();
        rb.velocity = Vector2.zero;
    }
}
