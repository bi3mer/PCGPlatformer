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
    public bool ProcedurallyGenerateLevels;
    public int N;
    public bool UsingSimplifiedNGram;
    public bool HeiarchalEnabled;
    public float HeiarchalMemory;
    public int MinLevelSize;
    public int MaxLevelSize;
    public bool UsingTieredGeneration;
    public float TieredGenerationMemoryUpdate;
    public bool DifficultyNGramEnabled;
    public float DifficultyNGramMemoryUpdate;
    public int DifficultyNGramLeftColumns;
    public int DifficultyNGramRightColumns;
}