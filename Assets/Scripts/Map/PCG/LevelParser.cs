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

        public static List<string> BreakColumnsIntoSimplifiedTokens(List<string> columns)
        {
            List<string> simplifiedTokens = new List<string>();

            foreach (string col in columns)
            {
                UnityEngine.Debug.Log(col);
            }

            return simplifiedTokens;
        }
    }
}