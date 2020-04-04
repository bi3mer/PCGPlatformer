using UnityStandardAssets._2D;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    public Camera2DFollow CameraFollow = null;
    public Button StartGameButton = null;
    public GameObject Grid = null;
    public Tilemap Tilemap = null;
    public int Level = 0;

    public LevelInfo LevelInfo = null;

    private void Awake()
    {
        Assert.IsNotNull(StartGameButton);
        Assert.IsNotNull(CameraFollow);
        Assert.IsNotNull(Tilemap);
        Assert.IsNotNull(Grid);
    }
}
