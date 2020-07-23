using System;
using System.Collections.Generic;

namespace PCG
{
    public static class LevelParser
    {
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
                returnColumns = BreakIntoSimplified(columns, 38);
            }
            else
            {
                returnColumns = BreakIntoSimplified(columns, columns[0].Length - 1);
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
                result = ClassifyColumn(column, 38);
            }
            else
            {
                result = ClassifyColumn(column, column.Length - 1);
            }

            return result;
        }

        private static string ClassifyColumn(string column, int startIndex)
        {
            string result;
            bool hasEnemies = false;
            bool hasPlatforms = false;

            for (int i = column.Length - 1; i >= 0; --i)
            {
                char token = column[i];
                if (token == TileChar.AcceleratingEnemyReverse || 
                    token == TileChar.AcceleratingEnemy        || 
                    token == TileChar.BasicEnemyReverse        || 
                    token == TileChar.BasicEnemy)
                {
                    hasEnemies = true;
                }
                else if (i != startIndex && token == TileChar.Block)
                {
                    hasPlatforms = true;
                }
            }

            // This if statement tests if the bottom most entry is not a block,
            // which means that the player must perform some kind of jump. It
            // also tests for the alternative situation where there is a block
            // in the rwo directly above the bottom. In this case the player 
            // will also have to jump.
            if (column[startIndex] != TileChar.Block ||
                (column[startIndex] == TileChar.Block &&
                 column[startIndex - 1] != TileChar.Empty &&
                 column[startIndex - 1].ToTile().IsEnemy() == false))
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