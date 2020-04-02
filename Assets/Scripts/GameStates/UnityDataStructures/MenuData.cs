using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class MenuData : MonoBehaviour
{
    [SerializeField]
    private Button startButton = null;

    private void Awake()
    {
        Assert.IsNotNull(startButton);
    }

    public void AddStartCallback(UnityAction callbacK)
    {
        startButton.onClick.AddListener(callbacK);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
