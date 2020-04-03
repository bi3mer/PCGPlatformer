using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

using UnityStandardAssets._2D;
using LightJson;

public static class LevelLoader
{
    public static void Build(List<List<string>> tiles, Tilemap tilemap, Camera2DFollow follow)
    {
        tilemap.ClearAllTiles();
        
        int x = 0;
        foreach (List<string> row in tiles)
        {
            int y = row.Count;
            foreach (string tile in row)
            {
                BuildTile(tile, x, y, tilemap, follow);
                --y;
            }

            ++x;
        }
    }

    public static void LoadAndBuild(string levelName, Tilemap tilemap, Camera2DFollow follow)
    {
        tilemap.ClearAllTiles();
        JsonArray matrix = Load(levelName);

        int y = matrix.Count;
        foreach (JsonArray row in matrix)
        {
            int x = 0;
            foreach (string tileString in row)
            {
                BuildTile(tileString, x, y, tilemap, follow);
                ++x;
            }

            --y;
        }
    }

    private static void BuildTile(string tileString, int x, int y, Tilemap tilemap, Camera2DFollow follow)
    {
        Tile tile = tileString.ToTile();
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
                go = Resources.Load<GameObject>("Prefabs/Character");
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
                go.GetComponent<Player>().LowestY = CalculateLowestY(tilemap);
            }
            else if (tile.NeedsMap())
            {
                go.GetComponent<BaseBehavior>().Map = tilemap;
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
        JsonArray matrix = Load(levelName);

        int y = matrix.Count;
        foreach (JsonArray row in matrix)
        {
            int x = 0;
            foreach (string tile in row)
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

    public static JsonArray Load(string levelName)
    {
        TextAsset text = Resources.Load<TextAsset>($"Levels/{levelName}");

        if (text == null)
        {
            Debug.LogWarning($"Level {levelName} was not found and cannot be loaded.");
            return null;
        }

        return JsonValue.Parse(text.text).AsJsonArray;
    }

    private static float CalculateLowestY(Tilemap tilemap)
    {
        return tilemap.CellToWorld(new Vector3Int(0, tilemap.cellBounds.yMin, 0)).y;
    }
}
