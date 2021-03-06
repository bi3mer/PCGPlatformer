﻿public enum Games
{ 
    Custom = 0,
    SuperMarioBros,
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
    public bool BackOffEnabled;
    public float BackOffMemory;
    public int LevelSize;
    public int MaxLevelSize;
    public bool UsingTieredGeneration;
    public float TieredGenerationMemoryUpdate;
    public bool DifficultyNGramEnabled;
    public float DifficultyNGramMemoryUpdate;
    public int DifficultyNGramLeftColumns;
    public int DifficultyNGramRightColumns;
}