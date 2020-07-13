using System.Collections.Generic;
using System.Diagnostics;

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
                returnColumns = BreakCustomLevelIntoSimplified(columns);
            }
            else
            {
                returnColumns = BreakVGLCLevelIntoSimplified(columns);            
            }

            return returnColumns;
        }

        private static List<string> BreakCustomLevelIntoSimplified(List<string> columns)
        {
            List<string> simplifiedTokens = new List<string>();

            foreach (string col in columns)
            {
                UnityEngine.Debug.Log(col);
            }

            return simplifiedTokens;
        }

        private static List<string> BreakVGLCLevelIntoSimplified(List<string> columns)
        {
            List<string> simplifiedTokens = new List<string>();

            foreach (string col in columns)
            {
                bool hasEnemies = false;
                bool hasPlatforms = false;

                for (int i = 0; i < col.Length; ++i)
                {
                    char token = col[i];
                    if (token == acrEnemy || token == acEnemy || token == brEnemy || token == bnemy)
                    {
                        hasEnemies = true;
                    }
                    else if (i > 0 && token == block)
                    {
                        hasPlatforms = true;
                    }
                }

                // This if statement tests if the bottom most entry is not a block,
                // which means that the player must perform some kind of jump. It
                // also tests for the alternative situation where there is a block
                // in the rwo directly above the bottom. In this case the player 
                // will also have to jump.
                if (col[0] == empty ||
                    (col[0] == block &&
                     col[1] != empty &&
                     col[1].ToTile().IsEnemy() == false))
                {
                    if (hasEnemies)
                    {
                        simplifiedTokens.Add(SimplifiedColumns.PlatformForcedEnemy);
                    }
                    else
                    {
                        simplifiedTokens.Add(SimplifiedColumns.PlatformForced);
                    }
                }
                else
                {
                    if (hasPlatforms)
                    {
                        if (hasEnemies)
                        {
                            simplifiedTokens.Add(SimplifiedColumns.PlatformOptionalEnemy);
                        }
                        else
                        {
                            simplifiedTokens.Add(SimplifiedColumns.PlatformOptional);
                        }
                    }
                    else
                    {
                        if (hasEnemies)
                        {
                            simplifiedTokens.Add(SimplifiedColumns.LinearEnemy);
                        }
                        else
                        {
                            simplifiedTokens.Add(SimplifiedColumns.Linear);
                        }
                    }
                }
            }

            return simplifiedTokens;
        }
    }
}