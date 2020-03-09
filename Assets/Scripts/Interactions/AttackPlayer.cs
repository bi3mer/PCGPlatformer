using UnityEngine.Assertions;
using UnityEngine;
using System;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

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
        if (collision.gameObject.tag.Equals(Tags.Player))
        {
            Debug.LogWarning("Cannot damage player yet!");
            hitPlayer();
        }
        else if (collision.gameObject.tag.Equals(Tags.Enemy))
        {
            hitEnemy();
        }
    }
}
