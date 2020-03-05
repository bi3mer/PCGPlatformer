using UnityEngine.Assertions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AttackPlayer))]
public class MoveBackAndForth : BaseBehavior
{
    [SerializeField]
    private float speed = 1f;

    private int direction = 1;
    private Vector3 velocity;
    private Rigidbody2D rb;

    private void Awake()
    {
        Assert.IsTrue(speed > 0);

        AttackPlayer attackPlayer = GetComponent<AttackPlayer>();
        Assert.IsNotNull(attackPlayer);

        attackPlayer.AddHitEnemyCallback(Collided);
        attackPlayer.AddHitPlayerCallback(Collided);

        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb);
    }

    private void FixedUpdate()
    {
        Vector3Int cellPos = Map.WorldToCell(transform.position);
        Vector3Int nextPos = new Vector3Int(cellPos.x + direction, cellPos.y, cellPos.z);

        if (Map.GetTile(nextPos) != null)
        {
            direction *= -1;
        }
        else
        {
            Vector3Int belowNextPos = new Vector3Int(nextPos.x, nextPos.y - 1, nextPos.z);

            if (Map.GetTile(belowNextPos) == null)
            {
                direction *= -1;
            }
        }

        Vector3 vec = new Vector3(speed * direction * Time.fixedDeltaTime, 0, 0);
        transform.position += vec;
    }

    private void Collided()
    {
        direction *= -1;
    }
}
