using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

public class LevelBeatenMenu : MonoBehaviour
{
    [SerializeField]
    private Button gotoMainMenuButton = null;

    [SerializeField]
    private Button replayLevelButton = null;

    [SerializeField]
    private Button gotoNextLevelButton = null;

    public Button GotoMainMenuButton { get { return gotoMainMenuButton; } }
    public Button ReplayLevelButton { get { return replayLevelButton; } }
    public Button GotoNextLevelButton { get { return gotoNextLevelButton; } }

    private void Awake()
    {
        Assert.IsNotNull(gotoMainMenuButton);
        Assert.IsNotNull(replayLevelButton);
        Assert.IsNotNull(gotoNextLevelButton);
    }
}
