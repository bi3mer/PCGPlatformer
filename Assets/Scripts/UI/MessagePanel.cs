using UnityEngine.Assertions;
using UnityEngine;
using TMPro;

using Tools.Mono;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(RectTransform))]
public class MessagePanel : Singleton<MessagePanel>
{
    [SerializeField]
    private TextMeshProUGUI title = null;

    [SerializeField]
    private TextMeshProUGUI body = null;

    [SerializeField]
    private Button button = null;

    public string Title
    {
        get { return title.text; }
        set { title.text = value; }
    }
    
    public string Body
    {
        get { return body.text; }
        set { body.text = value; }
    }

    public bool Active
    {
        get { return gameObject.activeSelf;  }
        set { gameObject.SetActive(value); }
    }

    public Action Callback;

    private void Awake()
    {
        Assert.IsNotNull(title);
        Assert.IsNotNull(body);
        Assert.IsNotNull(button);

        button.onClick.AddListener(() =>
        {
            Callback?.Invoke();
            Active = false;
        });

        RectTransform rt = GetComponent<RectTransform>();
        rt.offsetMax = Vector2.zero;
        rt.offsetMin = Vector2.zero;
    }

    private void Start()
    {
        Instance.Active = false;
    }
}
