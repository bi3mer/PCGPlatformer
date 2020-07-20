namespace PCG
{ 
    public static class LevelAnalyzer
    {
        public static double Linearity(string[] columns)
        {
            return 0;
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