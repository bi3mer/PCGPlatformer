using UnityEngine;
using System;

public class EndLevel : MonoBehaviour
{
    public Action PlayerWonCallback = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.gameObject.SetActive(false);

            if (PlayerWonCallback == null)
            {
                MessagePanel.Instance.Body = "You've reached an unexpected state. Please restart the game. Apologies.";
                MessagePanel.Instance.Active = true;
            }
            else
            {
                PlayerWonCallback.Invoke();
            }
        }
    }
}
