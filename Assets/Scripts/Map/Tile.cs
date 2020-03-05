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
}