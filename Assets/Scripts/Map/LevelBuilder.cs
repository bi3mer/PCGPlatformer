#if UNITY_EDITOR
using UnityEditor;

using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine;

using System.IO;

using LightJson;
using Boo.Lang;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField]
    private string levelName = null;

    private Tilemap tilemap;

    private void CheckValues()
    {
        Assert.IsNotNull(levelName);
        Assert.IsFalse(string.IsNullOrEmpty(levelName));

        if (tilemap == null)
        {
            tilemap = gameObject.GetComponentInChildren<Tilemap>();
            Assert.IsNotNull(tilemap);
        }
    }

    public void Save()
    {
        CheckValues();

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

        File.WriteAllText(path, GenerateMapRepresentation());
        AssetDatabase.Refresh();
    }

    public void Load()
    {
        CheckValues();
        LevelLoader.LoadAndBuildEditorOnly(levelName, tilemap);
    }

    public void Clear()
    {
        CheckValues();
        tilemap.ClearAllTiles();
    }

    private string GenerateMapRepresentation()
    {
        // load in the map from the tile map
        int xMin = tilemap.cellBounds.xMin;
        int xMax = tilemap.cellBounds.xMax;
        int yMin = tilemap.cellBounds.yMin;
        int yMax = tilemap.cellBounds.yMax;

        List<string> rows = new List<string>();

        for (int y = yMax; y >= yMin; --y)
        {
            string row = "";
            for (int x = xMin; x < xMax; ++x)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(tilePosition);
                string name;
                if (tile == null)
                {
                    name = "-";
                }
                else
                {
                    name = tile.name;
                }
                
                Tile t = name.NameToTile();
                Vector3Int position = new Vector3Int(x, y, 0);
                Vector3 rotation = tilemap.GetTransformMatrix(position).rotation.eulerAngles;

                if (rotation.Equals(Vector3.zero) == false)
                {
                    t = t.GetReverse();
                }

                row = $"{row}{t.ToMapString()}";
            }

            rows.Add(row);
        }

        RemoveRows(rows);
        return rows.Join("\n");
    }

    private void RemoveRows(List<string> rows)
    {
        // remove all empty rows starting from 0
        int rowsToRemove = 0;
        for (int y = 0; y < rows.Count; ++y)
        {
            string row = rows[y];
            bool removeRow = true;
            for (int x = 0; x < row.Length; ++x)
            {
                if (row[x] != '-')
                {
                    removeRow = false;
                    break;
                }
            }

            if (removeRow)
            {
                rowsToRemove += 1;
            }
            else
            {
                break;
            }
        }

        for (int i = 0; i < rowsToRemove; ++i)
        {
            rows.RemoveAt(0);
        }

        // remove all empty rows starting length - 1
        rowsToRemove = 0;
        for (int y = rows.Count - 1; y >= 0; --y)
        {
            string row = rows[y];
            bool removeRow = true;
            for (int x = 0; x < row.Length; ++x)
            {
                if (row[x] != '-')
                {
                    removeRow = false;
                    break;
                }
            }

            if (removeRow)
            {
                rowsToRemove += 1;
            }
            else
            {
                break;
            }
        }
        
        for (int i = 0; i < rowsToRemove; ++i)
        {
            rows.RemoveAt(rows.Count - 1);
        }
    }
}
#endif