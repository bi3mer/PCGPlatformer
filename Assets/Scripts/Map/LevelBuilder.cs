#if UNITY_EDITOR
using UnityEditor;

using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine;

using System.Collections.Generic;
using System.IO;

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

        File.WriteAllText(path, GenerateMap());
        AssetDatabase.Refresh();
    }

    public void Load()
    {
        CheckValues();
        LevelLoader.LoadAndBuildEditorOnly(levelName, tilemap);
    }

    private string GenerateMap()
    {
        // load in the map from the tile map
        int xMin = tilemap.cellBounds.xMin;
        int xMax = tilemap.cellBounds.xMax;
        int yMin = tilemap.cellBounds.yMin;
        int yMax = tilemap.cellBounds.yMax;

        List<List<char>> map = new List<List<char>>();

        for (int y = yMax; y >= yMin; --y)
        {
            List<char> row = new List<char>();
            for (int x = xMin; x < xMax; ++x)
            {
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile == null)
                {
                    row.Add(' ');
                }
                else
                {
                    row.Add(TileExtensions.NameToFile(tile.name).ToChar());
                }
            }

            map.Add(row);
        }

        // remove all empty rows starting from 0
        int rowsToRemove = 0;
        for (int y = 0; y < map.Count; ++y)
        {
            bool removeRow = true;
            for (int x = 0; x < map[y].Count; ++x)
            {
                if (map[y][x] != ' ')
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
            map.RemoveAt(0);
        }

        // remove all empty rows starting length - 1
        rowsToRemove = 0;
        for (int y = map.Count - 1; y >= 0; --y)
        {
            bool removeRow = true;
            for (int x = 0; x < map[y].Count; ++x)
            {
                if (map[y][x] != ' ')
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
            map.RemoveAt(map.Count - 1);
        }

        // build string and return
        string str = "";
        for (int y = 0; y < map.Count; ++y)
        {
            for (int x = 0; x < map[y].Count; ++x)
            {
                str += map[y][x];
            }

            str += '\n';
        }

        return str;
    }
}

#endif