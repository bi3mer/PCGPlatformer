using UnityEngine.Assertions;
using UnityEngine;
using System;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private bool onTriggerDamage = false;

    private Action hitPlayer;
    private Action hitEnemy;

#if UNITY_EDITOR
    private void Awake()
    {
        Assert.IsTrue(damage > 0);
    }
#endif

    public void AddHitPlayerCallback(Action callback)
    {
        hitPlayer += callback;
    }

    public void AddHitEnemyCallback(Action callback)
    {
        hitEnemy += callback;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onTriggerDamage == false)
        {
            RunHit(collision.gameObject.tag);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (onTriggerDamage)
        {
            RunHit(collision.tag);
        }
    }

    private void RunHit(string tag)
    {
        if (tag.Equals(Tags.Player))
        {
            hitPlayer?.Invoke();
        }
        else if (tag.Equals(Tags.Enemy))
        {
            hitEnemy?.Invoke();
        }
    }
}
