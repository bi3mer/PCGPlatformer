using System.Collections.Generic;

using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine;

using UnityStandardAssets._2D;
using LightJson;

public static class LevelLoader
{
    public static void LoadAndBuild(string levelName, Tilemap tilemap, Camera2DFollow follow)
    {
        tilemap.ClearAllTiles();
        JsonArray matrix = Load(levelName);

        foreach (JsonArray row in matrix)
        {
            foreach (JsonObject tileData in row)
            {
                Tile tile = tileData[MapSerializationKeys.Tile].AsString.ToTile();
                Vector3Int pos = tileData[MapSerializationKeys.Position].AsJsonObject.ToVector3Int();
                Vector3 rotation = tileData[MapSerializationKeys.Rotation].AsJsonObject.ToVector3();
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
                    case Tile.basicEnemy:
                        go = Resources.Load<GameObject>("Prefabs/BasicEnemy");
                        break;
                    case Tile.coin:
                        go = Resources.Load<GameObject>("Prefabs/Coin");
                        break;
                    case Tile.acceleratingEnemy:
                        go = Resources.Load<GameObject>("Prefabs/AcceleratingEnemy");
                        break;
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
                    go.transform.eulerAngles = rotation;

                    if (tile == Tile.playerOneStart)
                    {
                        follow.target = go.transform;
                        go.GetComponent<Player>().LowestY = CalculateLowestY(tilemap);
                    }
                    else if (tile.IsEnemyTile())
                    {
                        go.GetComponent<BaseBehavior>().Map = tilemap;
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
        JsonArray matrix = Load(levelName);

        foreach (JsonArray row in matrix)
        {
            foreach (JsonObject tileData in row)
            {
                // origin is at the top so we subtract height from y to get 
                // the level correctly oriented
                Tile tile = tileData[MapSerializationKeys.Tile].AsString.ToTile();
                Vector3Int pos = tileData[MapSerializationKeys.Position].AsJsonObject.ToVector3Int();
                Vector3 rotation = tileData[MapSerializationKeys.Rotation].AsJsonObject.ToVector3();

                TileBase tilePrefab = tile.GetPrefab();
                Quaternion rot = Quaternion.Euler(rotation);
                Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);

                tilemap.SetTile(pos, tilePrefab);
                tilemap.SetTransformMatrix(pos, mat);
            }
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
