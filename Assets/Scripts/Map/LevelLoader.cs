using System.Collections.Generic;

using UnityEngine.Tilemaps;
using UnityEngine;

using UnityStandardAssets._2D;

public static class LevelLoader
{
    public static void LoadAndBuild(string levelName, Tilemap tilemap, Camera2DFollow follow)
    {
        tilemap.ClearAllTiles();
        List<List<Tile>> tiles = Load(levelName);
        int h = tiles.Count;

        for (int y = 0; y < h; ++y)
        {
            for (int x = 0; x < tiles[y].Count; ++x)
            {
                Vector3Int pos = new Vector3Int(x, h - y, 0);
                Tile tile = tiles[y][x];
                GameObject go = null;


                switch (tile)
                {
                    case Tile.empty:
                    case Tile.block:
                    case Tile.crate:
                        tilemap.SetTile(pos, tiles[y][x].GetTile());
                        break;
                    case Tile.playerOneFinish:
                        go = Resources.Load<GameObject>($"Prefabs/EndGoal");
                        break;
                    case Tile.playerOneStart:
                        go = Resources.Load<GameObject>($"Prefabs/Character");
                        break;
                    case Tile.basicEnemy:
                        Debug.LogWarning("Enemy not in game.");
                        break;
                    case Tile.coin:
                        go = Resources.Load<GameObject>($"Prefabs/Coin");
                        break;
                    default:
                        Debug.LogWarning($"{tile} not found.");
                        break;
                }

                if (go != null)
                {
                    go = Object.Instantiate(go);
                    go.transform.position = tilemap.GetCellCenterWorld(pos);

                    if (tile == Tile.playerOneStart)
                    {
                        follow.target = go.transform;
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// This is only used in the level editor for loading everything into one 
    /// tile map. The function above separates it out so we get the behaviors
    /// in the game.
    /// </summary>
    /// <param name="levelName"></param>
    /// <param name="tilemap"></param>
    public static void LoadAndBuildEditorOnly(string levelName, Tilemap tilemap)
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
#endif

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
