using System.Collections.Generic;

using UnityEngine.Tilemaps;
using UnityEngine;

public static class LevelLoader
{
    public static void LoadAndBuild(string levelName, Tilemap tilemap)
    {
        tilemap.ClearAllTiles();
        List<List<Tile>> tiles = Load(levelName);
        int h = tiles.Count;

        for (int y = 0; y < h; ++y)
        {
            for (int x = 0; x < tiles[y].Count; ++x)
            {
                // origin is at the top so we subtract height from y to get 
                // the level correctly oriented
                tilemap.SetTile(new Vector3Int(x, h - y, 0), tiles[y][x].GetTile());
            }
        }
    }

    public static List<List<Tile>> Load(string levelName)
    {
        TextAsset text = Resources.Load<TextAsset>($"Levels/{levelName}");

        if (text == null)
        {
            Debug.LogWarning($"Level {levelName} was not found and cannot be loaded.");
            return null;
        }

        string[] content = text.text.Split('\n');

        return BuildMap(content);
    }

    private static List<List<Tile>> BuildMap(string[] content)
    {
        List<List<Tile>> map = new List<List<Tile>>();
        for (int y = 0; y < content.Length; ++y)
        {
            char[] charRow = content[y].ToCharArray();
            List<Tile> row = new List<Tile>();

            for (int x = 0; x < charRow.Length; ++x)
            {
                row.Add(charRow[x].CharToTile());
            }

            map.Add(row);
        }

        return map;
    }
}
