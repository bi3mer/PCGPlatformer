using UnityEngine.Assertions;
using UnityEngine;
using TMPro;

public class MessagePanel : Singleton<MessagePanel>
{
    [SerializeField]
    private TextMeshProUGUI title = null;

    [SerializeField]
    private TextMeshProUGUI body = null;

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

#if UNITY_EDITOR
    private void Awake()
    {
        Assert.IsNotNull(title);
        Assert.IsNotNull(body);
    }
#endif

    private void Start()
    {
        Instance.Active = false;
    }
}
