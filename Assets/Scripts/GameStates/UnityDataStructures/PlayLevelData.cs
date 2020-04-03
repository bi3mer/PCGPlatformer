using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine;

[RequireComponent(typeof(Tilemap))]
public class PlayLevelData : MonoBehaviour
{
    public Tilemap Tilemap { get; private set; }

    private void Awake()
    {
        Tilemap = GetComponent<Tilemap>();
        Assert.IsNotNull(Tilemap);
    }
}

//using UnityEngine.Assertions;
//using UnityEngine.Tilemaps;
//using UnityEngine;

//using UnityStandardAssets._2D;

//[RequireComponent(typeof(Tilemap))]
//public class LoadMap : MonoBehaviour
//{
//    [SerializeField]
//    private string levelName = null;

//    [SerializeField]
//    private Camera2DFollow cameraFollow = null;

//    private void Awake()
//    {
//        Tilemap tilemap = GetComponent<Tilemap>();

//        Assert.IsNotNull(cameraFollow);
//        Assert.IsNotNull(levelName);
//        Assert.IsNotNull(tilemap);

//        LevelLoader.LoadAndBuild(levelName, tilemap, cameraFollow);
//    }
//}
