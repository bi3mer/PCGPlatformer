using UnityEngine.Tilemaps;
using UnityEngine;
using System;

public enum Tile
{
    empty = 0,
    block,
    crate,
    playerOneStart,
    playerOneFinish,
    basicEnemy,
    basicEnemyReverse,
    acceleratingEnemy,
    acceleratingEnemyReverse,
    missileLauncher,
    missileLauncherReverse,
    coin
}

public static class TileExtensions
{
    public static Tile NameToTile(this string tileName)
    {
        switch (tileName)
        {
            case "-":
                return Tile.empty;
            case "Crate":
                return Tile.crate;
            case "Blocks_0":
                return Tile.block;
            case "Blocks_7":
                return Tile.playerOneStart;
            case "Blocks_8":
                return Tile.playerOneFinish;
            case "Gems_1":
                return Tile.coin;
            case "basic_enemy":
                return Tile.basicEnemy;
            case "basic_enemey_reverse":
                return Tile.basicEnemyReverse;
            case "accelerating_enemy":
                return Tile.acceleratingEnemy;
            case "accelerating_enemy_reverse":
                return Tile.acceleratingEnemyReverse;
            case "missile_launcher":
                return Tile.missileLauncher;
            case "missile_launcher_reverse":
                return Tile.missileLauncherReverse;
            default:
                throw new Exception($"{tileName} not found");
        }
    }

    public static string ToMapString(this Tile tile)
    {
        switch (tile)
        {
            case Tile.empty:
                return "-";
            case Tile.block:
                return "b";
            case Tile.crate:
                return "c";
            case Tile.playerOneStart:
                return "s";
            case Tile.playerOneFinish:
                return "f";
            case Tile.basicEnemy:
                return "A";
            case Tile.basicEnemyReverse:
                return "B";
            case Tile.acceleratingEnemy:
                return "C";
            case Tile.acceleratingEnemyReverse:
                return "D";
            case Tile.coin:
                return "$";
            case Tile.missileLauncher:
                return "M";
            case Tile.missileLauncherReverse:
                return "W";
            default:
                throw new Exception($"{tile} does not have valid to character entry");
        }
    }

    public static Tile ToTile(this char id)
    {
        switch (id)
        {
            case '-':
                return Tile.empty;
            case 'b':
                return Tile.block;
            case 'c':
                return Tile.crate;
            case 's':
                return Tile.playerOneStart;
            case 'f':
                return Tile.playerOneFinish;
            case 'A':
                return Tile.basicEnemy;
            case 'B':
                return Tile.basicEnemyReverse;
            case 'C':
                return Tile.acceleratingEnemy;
            case 'D':
                return Tile.acceleratingEnemyReverse;
            case '$':
                return Tile.coin;
            case 'M':
                return Tile.missileLauncher;
            case 'W':
                return Tile.missileLauncherReverse;
            default:
                Debug.Log($"|{id}| does not have valid to character entry");
                return Tile.empty;
        }
    }

    public static string GetName(this Tile tile)
    {
        switch (tile)
        {
            case Tile.empty:
                return "-";
            case Tile.block:
                return "Blocks_0";
            case Tile.crate:
                return "Crate";
            case Tile.playerOneStart:
                return "Blocks_7";
            case Tile.playerOneFinish:
                return "Blocks_8";
            case Tile.basicEnemyReverse:
            case Tile.basicEnemy:
                return "basic_enemy";
            case Tile.acceleratingEnemyReverse:
            case Tile.acceleratingEnemy:
                return "accelerating_enemy";
            case Tile.coin:
                return "Gems_1";
            case Tile.missileLauncherReverse:
            case Tile.missileLauncher:
                return "missile_launcher";
            default:
                Debug.LogWarning($"{tile} has no corresponding name.");
                return "";
        }
    }

    public static TileBase GetPrefab(this Tile tile)
    {
        switch (tile)
        {
            case Tile.empty:
                return null;
            case Tile.acceleratingEnemyReverse:
            case Tile.missileLauncherReverse:
            case Tile.basicEnemyReverse:
            case Tile.acceleratingEnemy:
            case Tile.missileLauncher:
            case Tile.playerOneFinish:
            case Tile.playerOneStart:
            case Tile.basicEnemy:
            case Tile.crate:
            case Tile.block:
            case Tile.coin:
                return Resources.Load<TileBase>($"Tiles/{tile.GetName()}");
            default:
                Debug.LogWarning($"{tile} not found.");
                return null;
        }
    }

    public static bool NeedsMap(this Tile tile)
    {
        switch (tile)
        {
            case Tile.acceleratingEnemyReverse:
            case Tile.basicEnemyReverse:
            case Tile.acceleratingEnemy:
            case Tile.basicEnemy:
                return true;
            case Tile.missileLauncherReverse:
            case Tile.playerOneFinish:
            case Tile.missileLauncher:
            case Tile.playerOneStart:
            case Tile.empty:
            case Tile.crate:
            case Tile.block:
            case Tile.coin:
            default:
                return false;
        }
    }

    public static Tile GetReverse(this Tile tile)
    {
        switch (tile)
        {
            case Tile.acceleratingEnemy:
                return Tile.acceleratingEnemyReverse;
            case Tile.missileLauncher:
                return Tile.missileLauncherReverse;
            case Tile.basicEnemy:
                return Tile.basicEnemyReverse;
            case Tile.empty:
            case Tile.block:
            case Tile.crate:
            case Tile.playerOneStart:
            case Tile.playerOneFinish:
            case Tile.coin:
                return tile;
            case Tile.acceleratingEnemyReverse:
            case Tile.missileLauncherReverse:
            case Tile.basicEnemyReverse:
            default:
                Debug.LogError($"{tile} not found in Tile.GetReverse");
                return Tile.empty;
        }
    }

    public static bool IsReverseTile(this Tile tile)
    {
        switch (tile)
        {
            case Tile.acceleratingEnemyReverse:
            case Tile.missileLauncherReverse:
            case Tile.basicEnemyReverse:
                return true;
            case Tile.empty:
            case Tile.block:
            case Tile.crate:
            case Tile.playerOneStart:
            case Tile.playerOneFinish:
            case Tile.basicEnemy:
            case Tile.acceleratingEnemy:
            case Tile.missileLauncher:
            case Tile.coin:
            default:
                return false;
        }
    }
}