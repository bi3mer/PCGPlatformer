using System;

public enum Tile
{
    block,
    crate,
    playerOneStart,
    playerOneFinish,
    basicEnemy
}

public static class TileExtensions
{
    public static Tile ToTile(string tileName)
    {
        switch (tileName)
        {
            case "Crate":
                return Tile.crate;
            case "Blocks_0":
                return Tile.block;
            case "Blocks_7":
                return Tile.playerOneStart;
            case "Blocks_8":
                return Tile.playerOneFinish;
            default:
                throw new Exception($"{tileName} not found");
        }
    }

    public static char ToCharacter(this Tile tile)
    {
        switch (tile)
        {
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
            default:
                throw new Exception($"{tile} does not have valid to character entry");
        }
    }
}