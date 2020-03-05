using UnityEngine;

public class StompBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals(Tags.Player))
        {
            Destroy(gameObject);
        }
    }
}
