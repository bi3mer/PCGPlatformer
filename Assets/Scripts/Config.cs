public enum Games
{ 
    Custom = 0,
    SuperMariosBros,
    SuperMarioBros2,
    SuperMarioBros2Japan,
    SuperMarioLand
}

public class Config
{
    public Games Game;
    public int N;
    public bool UsingSimplifiedNGram;
    public int MinLevelSize;
    public int MaxLevelSize;
    public bool UsingTieredGeneration;
    public float TieredGenerationMemoryUpdate;
    public bool DifficultyNGramEnabled;
    public float DifficultyNGramMemoryUpdate;
    public float DifficultyNGramLeftColumns;
    public float DifficultyNGramRightColumns;
}