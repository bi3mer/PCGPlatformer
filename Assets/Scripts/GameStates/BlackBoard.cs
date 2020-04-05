using UnityStandardAssets._2D;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    public Camera2DFollow CameraFollow = null;
    public Button TryLevelAgainButton = null;
    public Button StartGameButton = null;

    public Button PlayLevelAgainButton = null;
    public Button GotoNextLevelButton = null;

    public GameObject Grid = null;
    public Tilemap Tilemap = null;
    public int Level = 0;

    public LevelInfo LevelInfo = null;

    private void Awake()
    {
        Assert.IsNotNull(CameraFollow);
        Assert.IsNotNull(TryLevelAgainButton);
        Assert.IsNotNull(StartGameButton);

        Assert.IsNotNull(PlayLevelAgainButton);
        Assert.IsNotNull(GotoNextLevelButton);

        Assert.IsNotNull(Tilemap);
        Assert.IsNotNull(Grid);
    }
}
