#if UNITY_EDITOR
using UnityEditor;

using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine;

using System.Collections.Generic;
using System.IO;

using LightJson;

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

        List<List<char>> map = new List<List<char>>();
        List<Vector3Int> specialTiles = new List<Vector3Int>();

        for (int y = yMax; y >= yMin; --y)
        {
            List<char> row = new List<char>();
            for (int x = xMin; x < xMax; ++x)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(tilePosition);
                if (tile == null)
                {
                    row.Add(' ');
                }
                else
                {
                    Tile t = TileExtensions.NameToFile(tile.name);
                    if (t.IsEnemyTile())
                    {
                        specialTiles.Add(tilePosition);
                    }

                    row.Add(t.ToChar());
                }
            }

            map.Add(row);
        }

        JsonArray tileData = new JsonArray();
        foreach (Vector3Int pos in specialTiles)
        {
            Quaternion rotation = tilemap.GetTransformMatrix(pos).rotation;
            tileData.Add(new JsonObject
            {
                { MapSerializationKeys.TileDataKeys.Position, pos.ToJsonObject() },
                { MapSerializationKeys.TileDataKeys.Rotation, rotation.ToJsonObject() }
            });
        }

        JsonObject data = new JsonObject
        {
            { MapSerializationKeys.Map, MapToString(map) },
            { MapSerializationKeys.TileData, tileData }
        };

        return data.ToString(true);
    }

    private void RemoveRows(List<List<char>> map)
    {
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
    }

    private string MapToString(List<List<char>> map)
    {
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