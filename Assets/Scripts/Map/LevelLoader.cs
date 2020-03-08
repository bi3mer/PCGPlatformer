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
        Debug.LogError("uncomment and reimplement this!");
        //tilemap.ClearAllTiles();
        //JsonObject data = Load(levelName);
        ////List<List<Tile>> tiles = Load(levelName);
        //int h = tiles.Count;

        //for (int y = 0; y < h; ++y)
        //{
        //    for (int x = 0; x < tiles[y].Count; ++x)
        //    {
        //        Vector3Int pos = new Vector3Int(x, h - y, 0);
        //        Tile tile = tiles[y][x];
        //        GameObject go = null;


        //        switch (tile)
        //        {
        //            case Tile.empty:
        //            case Tile.block:
        //            case Tile.crate:
        //                tilemap.SetTile(pos, (TileBase)tiles[y][x].GetTile());
        //                break;
        //            case Tile.playerOneFinish:
        //                go = Resources.Load<GameObject>("Prefabs/EndGoal");
        //                break;
        //            case Tile.playerOneStart:
        //                go = Resources.Load<GameObject>("Prefabs/Character");
        //                break;
        //            case Tile.basicEnemy:
        //                go = Resources.Load<GameObject>("Prefabs/BasicEnemy");
        //                break;
        //            case Tile.coin:
        //                go = Resources.Load<GameObject>("Prefabs/Coin");
        //                break;
        //            case Tile.acceleratingEnemy:
        //                go = Resources.Load<GameObject>("Prefabs/AcceleratingEnemy");
        //                break;
        //            default:
        //                Debug.LogWarning($"{tile} not found.");
        //                break;
        //        }

        //        if (go != null)
        //        {
        //            go = Object.Instantiate(go);
        //            go.transform.position = tilemap.GetCellCenterWorld(pos);

        //            if (tile == Tile.playerOneStart)
        //            {
        //                follow.target = go.transform;
        //                go.GetComponent<Player>().LowestY = CalculateLowestY(tilemap, h);
        //            }
        //            else if(tile.IsEnemyTile())
        //            {
        //                go.GetComponent<BaseBehavior>().Map = tilemap;
        //            }
        //        }
        //    }
        //}
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

        int h = matrix.Count;

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

    //private static List<List<Tile>> BuildMap(string[] content)
    //{
    //    List<List<Tile>> map = new List<List<Tile>>();
    //    for (int y = 0; y < content.Length; ++y)
    //    {
    //        char[] charRow = content[y].ToCharArray();
    //        List<Tile> row = new List<Tile>();

    //        for (int x = 0; x < charRow.Length; ++x)
    //        {
    //            row.Add(charRow[x].CharToTile());
    //        }

    //        map.Add(row);
    //    }

    //    return map;
    //}

    private static float CalculateLowestY(Tilemap tilemap, int h)
    {
        return tilemap.CellToWorld(new Vector3Int(0, 0, 0)).y;
    }
}
