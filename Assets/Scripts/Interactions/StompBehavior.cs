using UnityEngine;
using UnityStandardAssets._2D;

public class StompBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals(Tags.Player))
        {
            collision.GetComponent<PlatformerCharacter2D>().Jump(true);

            Destroy(gameObject);
        }
    }
}
