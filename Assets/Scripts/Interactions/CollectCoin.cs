using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals(Tags.Player))
        {
            //Debug.LogWarning("Coin collecting not fully implemented");
            Destroy(gameObject);
        }
    }
}
