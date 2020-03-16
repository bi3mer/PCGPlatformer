using UnityEngine.Assertions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AttackPlayer))]
public class MoveBackAndForth : BaseBehavior
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

        attackPlayer.AddHitEnemyCallback(Flip);
        attackPlayer.AddHitPlayerCallback(Flip);

        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb);
    }

    private void FixedUpdate()
    {
        Vector3Int cellPos = Map.WorldToCell(transform.position);
        Vector3Int nextPos = new Vector3Int(cellPos.x + Direction, cellPos.y, cellPos.z);

        if (Map.GetTile(nextPos) != null)
        {
            Flip();
        }
        else
        {
            Vector3Int belowNextPos = new Vector3Int(nextPos.x, nextPos.y - 1, nextPos.z);

            if (Map.GetTile(belowNextPos) == null)
            {
                Flip();
            }
        }

        Vector3 vec = new Vector3(speed * Direction * Time.fixedDeltaTime, 0, 0);
        transform.position += vec;
    }
}
