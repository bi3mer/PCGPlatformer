using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    [SerializeField]
    private Button gotoMainMenuButton = null;

    [SerializeField]
    private Button replayLevelButton = null;

    public Button GotoMainMenuButton { get { return gotoMainMenuButton; } }
    public Button ReplayLevelButton { get { return replayLevelButton; } }

    private void Awake()
    {
        Assert.IsNotNull(gotoMainMenuButton);
        Assert.IsNotNull(replayLevelButton);
    }
}
