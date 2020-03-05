using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine;

[RequireComponent(typeof(Tilemap))]
public class LoadMap : MonoBehaviour
{
    [SerializeField]
    private string levelName = null;

    private void Awake()
    {
        Tilemap tilemap = GetComponent<Tilemap>();

        Assert.IsNotNull(levelName);
        Assert.IsNotNull(tilemap);

        LevelLoader.LoadAndBuild(levelName, tilemap);
    }
}
