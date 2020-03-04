#if UNITY_EDITOR
using UnityEditor;

using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine;

using System.IO;

public class GenerateMapFromTiles : MonoBehaviour
{
    [SerializeField]
    private string levelName = null;

    private Tilemap tilemap;

    void Start()
    {
        Assert.IsNotNull(levelName);
        Assert.IsFalse(string.IsNullOrEmpty(levelName));

        tilemap = GetComponent<Tilemap>();
        Assert.IsNotNull(tilemap);

        string path = Path.Combine("Assets", "Resources", "Levels", $"{levelName}.txt");
        if (File.Exists(path))
        {
            bool clickedYes = EditorUtility.DisplayDialog(
                "File Found",
                $"A level with the name of {levelName} already exists. Do you want to replace it?",
                "yes",
                "no");

            if (clickedYes == false)
            {
                return;
            }
        }
        Debug.Log("here!");

        File.WriteAllText(path, GenerateMap());
        AssetDatabase.Refresh();
    }

    private string GenerateMap()
    {
        int xMin = tilemap.cellBounds.xMin;
        int xMax = tilemap.cellBounds.xMax;
        int yMin = tilemap.cellBounds.yMin;
        int yMax = tilemap.cellBounds.yMax;

        string map = "";

        for (int y = yMax; y >= yMin; --y)
        {
            for (int x = xMin; x < xMax; ++x)
            {
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile == null)
                {
                    map += " ";
                }
                else
                {
                    map += TileExtensions.ToTile(tile.name).ToCharacter();
                }
            }

            map += "\n";
        }

        return map;
    }
}

#endif