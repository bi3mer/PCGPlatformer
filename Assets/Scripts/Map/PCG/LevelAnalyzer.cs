using System.Collections.Generic;

namespace PCG
{ 
    public static class LevelAnalyzer
    {
        // @NOTE: this could be improved by using linear regression on the result
        // but implementing is more pain than it's worth. Instead, it's done on 
        // python/analysis side.
        public static List<int> Positions(string[] columns)
        {
            List<int> positions = new List<int>();

            foreach (string col in columns)
            {
                int position = -1;
                for (int i = 1; i < col.Length; ++i)
                {
                    Tile neighbor = col[i - 1].ToTile();
                    if (col[i].Equals(TileChar.Block) &&
                       (neighbor == Tile.empty || neighbor.IsEnemy()))
                    {
                        position = col.Length - i;
                        break;
                    }
                }

                if (position != -1)
                {
                    positions.Add(position);
                }
            }
            
            return positions;
        }

        public static double Leniency(string[] simplified)
        {
            double score = 0;
            foreach (string token in simplified)
            {
                switch (token)
                {
                    case SimplifiedColumns.LinearEnemy:
                        score += 0.5;
                        break;
                    case SimplifiedColumns.PlatformOptional:
                        score += 0.10;
                        break;
                    case SimplifiedColumns.PlatformOptionalEnemy:
                        score += 0.6;
                        break;
                    case SimplifiedColumns.PlatformForced:
                        score += 0.5;
                        break;
                    case SimplifiedColumns.PlatformForcedEnemy:
                        score += 1.0;
                        break;
                    case SimplifiedColumns.Linear:
                    default:
                        break;
                }
            }

            return score;
        }
    }
}