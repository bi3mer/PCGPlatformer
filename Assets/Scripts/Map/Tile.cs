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
    acceleratingEnemy,
    coin
}

public static class TileExtensions
{
    public static Tile NameToFile(string tileName)
    {
        switch (tileName)
        {
            case "":
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
            case "accelerating_enemy":
                return Tile.acceleratingEnemy;
            default:
                throw new Exception($"{tileName} not found");
        }
    }

    public static char ToChar(this Tile tile)
    {
        switch (tile)
        {
            case Tile.empty:
                return ' ';
            case Tile.block:
                return 'b';
            case Tile.crate:
                return 'c';
            case Tile.playerOneStart:
                return 's';
            case Tile.playerOneFinish:
                return 'f';
            case Tile.basicEnemy:
                return 'B';
            case Tile.acceleratingEnemy:
                return 'A';
            case Tile.coin:
                return '$';
            default:
                throw new Exception($"{tile} does not have valid to character entry");
        }
    }

    public static Tile CharToTile(this char character)
    {
        switch (character)
        {
            case ' ':
                return Tile.empty;
            case 'b':
                return Tile.block;
            case 'c':
                return Tile.crate;
            case 's':
                return Tile.playerOneStart;
            case 'f':
                return Tile.playerOneFinish;
            case 'B':
                return Tile.basicEnemy;
            case 'A':
                return Tile.acceleratingEnemy;
            case '$':
                return Tile.coin;
            default:
                throw new Exception($"{character} does not have valid to character entry");
        }
    }

    public static string GetName(this Tile tile)
    {
        switch (tile)
        {
            case Tile.empty:
                return "";
            case Tile.block:
                return "Blocks_0";
            case Tile.crate:
                return "Crate";
            case Tile.playerOneStart:
                return "Blocks_7";
            case Tile.playerOneFinish:
                return "Blocks_8";
            case Tile.basicEnemy:
                return "basic_enemy";
            case Tile.acceleratingEnemy:
                return "accelerating_enemy";
            case Tile.coin:
                return "Gems_1";
            default:
                Debug.LogWarning($"{tile} has no corresponding name.");
                return "";
        }
    }

    public static TileBase GetTile(this Tile tile)
    {
        switch (tile)
        {
            case Tile.empty:
                return null;
            case Tile.acceleratingEnemy:
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

    public static bool IsEnemyTile(this Tile tile)
    {
        return tile == Tile.basicEnemy || tile == Tile.acceleratingEnemy;
    }
}