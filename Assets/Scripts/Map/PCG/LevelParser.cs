using System.Collections.Generic;
using UnityEngine.Assertions;
using LightJson;
using Tools.ID;

namespace PCG
{
    public class LevelParser
    {
        private Dictionary<string, string> columnToId = null;
        private Dictionary<string, string> idToColumn = null;
        private IEnumerator<string> idGenerator = null;

        public LevelParser(int idSize = 15)
        {
            columnToId = new Dictionary<string, string>();
            idToColumn = new Dictionary<string, string>();
            idGenerator = StringIDGenerator.GetID(idSize).GetEnumerator();
        }

        public List<string> GetLevelTokens(string levelName)
        {
            JsonArray map = Utility.Load(levelName);
            List<string> columns = BreakMapIntoColumns(map);
            return BuildTokens(columns);
        }

        public string GetColumn(string id)
        {
            Assert.IsTrue(idToColumn.ContainsKey(id));
            return idToColumn[id];
        }

        public string GetId(string column)
        {
            Assert.IsTrue(columnToId.ContainsKey(column));
            return columnToId[column];
        }

        private List<string> BreakMapIntoColumns(JsonArray map)
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

        private List<string> BuildTokens(List<string> columns)
        {
            List<string> tokens = new List<string>();
            foreach (string column in columns)
            {
                if (columnToId.ContainsKey(column))
                {
                    tokens.Add(columnToId[column]);
                }
                else
                {
                    idGenerator.MoveNext();
                    string id = idGenerator.Current;

                    columnToId.Add(column, id);
                    idToColumn.Add(id, column);
                    tokens.Add(id);
                }
            }
            
            return tokens;
        }
    }
}