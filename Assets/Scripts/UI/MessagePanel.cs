using UnityEngine.Assertions;
using UnityEngine;
using TMPro;

using Tools.Mono;
using UnityEngine.UI;

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

    private void Awake()
    {
        Assert.IsNotNull(title);
        Assert.IsNotNull(body);
        Assert.IsNotNull(button);

        button.onClick.AddListener(() =>
        {
            Active = false;
        });
    }

    private void Start()
    {
        Instance.Active = false;
    }
}
