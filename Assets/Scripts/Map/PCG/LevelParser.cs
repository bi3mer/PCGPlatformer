﻿using System.Collections.Generic;
using LightJson;

namespace PCG
{
    public static class LevelParser
    {
        public static List<string> BreakMapIntoColumns(string levelName)
        {
            JsonArray map = Utility.Load(levelName);
            return BreakMapIntoColumns(map);
        }

        private static List<string> BreakMapIntoColumns(JsonArray map)
        {
            List<string> columns = new List<string>();
            for (int y = 0; y < map.Count; ++y)
            {
                JsonArray row = map[y].AsJsonArray;
                if (y == 0)
                {
                    for (int x = 0; x < row.Count; ++x)
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

        //private List<string> BuildTokens(List<string> columns)
        //{
        //    List<string> tokens = new List<string>();
        //    foreach (string column in columns)
        //    {
        //        if (columnToId.ContainsKey(column))
        //        {
        //            tokens.Add(columnToId[column]);
        //        }
        //        else
        //        {
        //            idGenerator.MoveNext();
        //            string id = idGenerator.Current;

        //            columnToId.Add(column, id);
        //            idToColumn.Add(id, column);
        //            tokens.Add(id);
        //        }
        //    }
            
        //    return tokens;
        //}
    }
}