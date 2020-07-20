using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

using UnityStandardAssets._2D;
using System.Linq;

public static class LevelLoader
{
    public static LevelInfo Build(List<List<char>> tiles, Tilemap tilemap, Camera2DFollow follow)
    {
        LevelInfo levelInfo = new LevelInfo();
        tilemap.ClearAllTiles();
        
        int x = 0;
        foreach (List<char> row in tiles)
        {
            int y = row.Count;
            foreach (char tile in row)
            {
                BuildTile(tile, x, y, tilemap, follow, levelInfo);
                --y;
            }

            ++x;
        }

        return levelInfo;
    }

    public static LevelInfo LoadAndBuild(string levelName, Tilemap tilemap, Camera2DFollow follow)
    {
        LevelInfo li = new LevelInfo();
        tilemap.ClearAllTiles();
        string[] rows = Load(levelName);

        int y = rows.Length;
        foreach (string row in rows)
        {
            int x = 0;
            foreach (char tileString in row)
            {
                BuildTile(tileString, x, y, tilemap, follow, li);
                ++x;
            }

            --y;
        }

        return li;
    }

    private static void BuildTile(char tileChar, int x, int y, Tilemap tilemap, Camera2DFollow follow, LevelInfo levelInfo)
    {
        Tile tile = tileChar.ToTile();
        Vector3Int pos = new Vector3Int(x, y, 0);
        GameObject go = null;

        switch (tile)
        {
            case Tile.empty:
            case Tile.block:
            case Tile.crate:
                tilemap.SetTile(pos, tile.GetPrefab());
                break;
            case Tile.playerOneFinish:
                go = Resources.Load<GameObject>("Prefabs/EndGoal");
                break;
            case Tile.playerOneStart:
                if (levelInfo.Player == null)
                {
                    go = Resources.Load<GameObject>("Prefabs/Character");
                }
                break;
            case Tile.basicEnemyReverse:
            case Tile.basicEnemy:
                go = Resources.Load<GameObject>("Prefabs/BasicEnemy");
                break;
            case Tile.coin:
                go = Resources.Load<GameObject>("Prefabs/Coin");
                break;
            case Tile.acceleratingEnemyReverse:
            case Tile.acceleratingEnemy:
                go = Resources.Load<GameObject>("Prefabs/AcceleratingEnemy");
                break;
            case Tile.missileLauncherReverse:
            case Tile.missileLauncher:
                go = Resources.Load<GameObject>("Prefabs/MissileLauncher");
                break;
            default:
                Debug.LogWarning($"{tile} not found.");
                break;
        }

        if (go != null)
        {
            go = Object.Instantiate(go);
            go.transform.position = tilemap.GetCellCenterWorld(pos);

            if (tile.IsReverseTile())
            {
                go.transform.eulerAngles = new Vector3(0, 180, 0);
            }

            if (tile == Tile.playerOneStart)
            {
                follow.target = go.transform;
                levelInfo.Player = go.GetComponent<Player>();
                levelInfo.Player.LowestY = CalculateLowestY(tilemap);
            }
            else if (tile.NeedsMap())
            {
                go.GetComponent<BaseBehavior>().Map = tilemap;
                levelInfo.Enemies.Add(go);
            }
            else if (tile == Tile.playerOneFinish)
            {
                levelInfo.EndLevelTiles.Add(go.GetComponent<EndLevel>());
            }
            else if (tile == Tile.missileLauncher || tile == Tile.missileLauncherReverse)
            {
                levelInfo.Turrets.Add(go);
            }
            else if (tile == Tile.coin)
            {
                levelInfo.Coins.Add(go.GetComponent<CollectCoin>());
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
        string[] matrix = Load(levelName);

        int y = matrix.Length;
        foreach (string row in matrix)
        {
            int x = 0;
            foreach (char tile in row)
            {
                Tile t = tile.ToTile();

                TileBase tilePrefab = t.GetPrefab();
                Vector3Int pos = new Vector3Int(x, y, 0);
                tilemap.SetTile(pos, tilePrefab);

                if (t.IsReverseTile())
                {
                    Quaternion rot = Quaternion.Euler(new Vector3(0, 180, 0));
                    Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
                    tilemap.SetTransformMatrix(pos, mat);
                }

                ++x;
            }

            --y;
        }
    }
#endif

    public static string[] Load(string levelName)
    {
        TextAsset text = Resources.Load<TextAsset>($"Levels/{levelName}");

        if (text == null)
        {
            Debug.LogWarning($"Level {levelName} was not found and cannot be loaded.");
            return null;
        }


        return text.text.Split('\n');
    }

    public static List<string> LoadIntoColumns(string levelName)
    {
        string[] rows = Load(levelName);
        List<string> columns = new List<string>(rows[0].Length);

        for (int i = 0; i < rows.Length; ++i)
        {

            string row = rows[i];
            for (int j = 0; j < row.Length; ++j)
            {
                if (i == 0)
                {
                    columns.Add("");
                }

                columns[j] += row[j];
            }
        }

        return columns;
    }

    private static float CalculateLowestY(Tilemap tilemap)
    {
        return tilemap.CellToWorld(new Vector3Int(0, tilemap.cellBounds.yMin, 0)).y;
    }
}
