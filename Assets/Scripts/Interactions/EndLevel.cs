using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.gameObject.SetActive(false);
            MessagePanel.Instance.Active = true;
        }
    }
}
