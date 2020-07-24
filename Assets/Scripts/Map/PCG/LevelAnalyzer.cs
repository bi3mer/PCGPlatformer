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

                // if -1, the parser needs to skip in the analysis step
                positions.Add(position);
            }
            
            return positions;
        }

        // for context 1 is easiest and that is linear. ) is hardest and that
        // is where a platform is forced and there is an enemy.
        public static double Leniency(string[] simplified)
        {
            double score = 0;
            foreach (string token in simplified)
            {
                switch (token)
                {
                    case SimplifiedColumns.Linear:
                        score += 1.0;
                        break;
                    case SimplifiedColumns.LinearEnemy:
                        score += 0.5;
                        break;
                    case SimplifiedColumns.PlatformOptional:
                        score += 0.9;
                        break;
                    case SimplifiedColumns.PlatformOptionalEnemy:
                        score += 0.4;
                        break;
                    case SimplifiedColumns.PlatformForced:
                        score += 0.5;
                        break;
                    case SimplifiedColumns.PlatformForcedEnemy:
                        // + 0
                        break;
                    default:
                        UnityEngine.Debug.LogError($"{token} is unknown.");
                        break;
                }
            }

            return score;
        }
    }
}