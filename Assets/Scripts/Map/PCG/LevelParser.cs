using System.Collections.Generic;
using LightJson;

namespace PCG
{
    public static class LevelParser
    {
        public static List<string> BreakMapIntoColumns(string levelName)
        {
            JsonArray map = Utility.Load(levelName);
            List<string> columns = new List<string>();

            for (int y = 0; y < map.Count; ++y)
            {
                JsonArray row = map[y].AsJsonArray;
                if (y == 0)
                {                   for (int x = 0; x < row.Count; ++x)
                    {
                        columns.Add(row[x].AsString);
                    }
                }
                else
                {
                    for (int x = 0; x < row.Count; ++x)
                    {
                        columns[x] += row[x].AsString;
                    }
                }
            }

            return columns;
        }
    }
}