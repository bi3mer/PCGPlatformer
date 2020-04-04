using UnityEngine.Assertions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AttackPlayer))]
public class MissileConstantSpeed : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float lifeTime = 10f;

    private Rigidbody2D rb = null;
    private float time = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb);

        AttackPlayer attack = GetComponent<AttackPlayer>();
        Assert.IsNotNull(attack);

        attack.AddHitPlayerCallback(HitPlayer);
    }

    private void Start()
    {
        rb.velocity = new Vector2(speed * transform.forward.z, 0);
    }

    private void Update()
    {
        if (time > lifeTime)
        {
            Destroy(gameObject);
        }
        time += Time.deltaTime;
    }

    private void HitPlayer()
    {
        Destroy(gameObject);
    }
}
