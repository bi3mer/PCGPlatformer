using System;
using System.Collections.Generic;
using System.Linq;

namespace PCG
{
    public static class LevelParser
    {
        private static readonly char acrEnemy = Tile.acceleratingEnemyReverse.ToChar();
        private static readonly char acEnemy = Tile.acceleratingEnemy.ToChar();
        private static readonly char brEnemy = Tile.basicEnemyReverse.ToChar();
        private static readonly char bnemy = Tile.basicEnemy.ToChar();
        private static readonly char empty = Tile.empty.ToChar();
        private static readonly char block = Tile.block.ToChar();

        public static List<string> BreakMapIntoColumns(string levelName)
        {
            string[] map = Utility.Load(levelName);
            List<string> columns = new List<string>();

            for (int y = 0; y < map.Length; ++y)
            {
                string row = map[y];
                if (y == 0)
                {                   
                    for (int x = 0; x < row.Length; ++x)
                    {
                        columns.Add(row[x].ToString());
                    }
                }
                else
                {
                    for (int x = 0; x < row.Length; ++x)
                    {
                        columns[x] += row[x];
                    }
                }
            }

            return columns;
        }

        public static List<string> BreakColumnsIntoSimplifiedTokens(List<string> columns, bool isCustom)
        {
            List<string> returnColumns;
            if (isCustom)
            {
                returnColumns = BreakIntoSimplified(columns, 17);
            }
            else
            {
                returnColumns = BreakIntoSimplified(columns, 0);            
            }

            return returnColumns;
        }

        private static List<string> BreakIntoSimplified(List<string> columns, int startIndex)
        {
            List<string> simplifiedTokens = new List<string>();

            foreach (string col in columns)
            {
                simplifiedTokens.Add(ClassifyColumn(col, startIndex));
            }

            return simplifiedTokens;
        }

        public static string ClassifyColumn(string column, Games game)
        {
            string result;
            if (game == Games.Custom)
            {
                result = ClassifyColumn(column, 17);
            }
            else
            {
                result = ClassifyColumn(column, 0);
            }

            return result;
        }

        private static string ClassifyColumn(string column, int startIndex)
        {
            char[] toReverse = column.ToCharArray();
            Array.Reverse(toReverse);
            string col = new string(toReverse);

            string result;
            bool hasEnemies = false;
            bool hasPlatforms = false;

            for (int i = 0; i < col.Length; ++i)
            {
                char token = col[i];
                if (token == acrEnemy || token == acEnemy || token == brEnemy || token == bnemy)
                {
                    hasEnemies = true;
                }
                else if (i != startIndex && token == block)
                {
                    hasPlatforms = true;
                }
            }

            // This if statement tests if the bottom most entry is not a block,
            // which means that the player must perform some kind of jump. It
            // also tests for the alternative situation where there is a block
            // in the rwo directly above the bottom. In this case the player 
            // will also have to jump.
            if (col[startIndex] != block ||
                (col[startIndex] == block &&
                 col[startIndex + 1] != empty &&
                 col[startIndex + 1].ToTile().IsEnemy() == false))
            {
                if (hasEnemies)
                {
                    result = SimplifiedColumns.PlatformForcedEnemy;
                }
                else
                {
                    result = SimplifiedColumns.PlatformForced;
                }
            }
            else
            {
                if (hasPlatforms)
                {
                    if (hasEnemies)
                    {
                        result = SimplifiedColumns.PlatformOptionalEnemy;
                    }
                    else
                    {
                        result = SimplifiedColumns.PlatformOptional;
                    }
                }
                else
                {
                    if (hasEnemies)
                    {
                        result = SimplifiedColumns.LinearEnemy;
                    }
                    else
                    {
                        result = SimplifiedColumns.Linear;
                    }
                }
            }

            return result;
        }
    }
}