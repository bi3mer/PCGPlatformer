#if UNITY_EDITOR
using UnityEngine.Assertions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateMapFromTiles : MonoBehaviour
{
    private Tilemap tilemap;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        Assert.IsNotNull(tilemap);


        int xMin = tilemap.cellBounds.xMin;
        int xMax = tilemap.cellBounds.xMax;
        int yMin = tilemap.cellBounds.yMin;
        int yMax = tilemap.cellBounds.yMax;

        string test = "";

        for (int y = yMax; y >= yMin; --y)
        {
            for (int x = xMin; x < xMax; ++x)
            {
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile == null)
                {
                    test += " ";
                }
                else
                {
                    test += TileExtensions.ToTile(tile.name).ToCharacter();
                }
            }

            test += "\n";
        }

        Debug.Log(test);
    }
}

#endif