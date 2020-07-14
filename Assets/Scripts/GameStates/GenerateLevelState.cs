using System.Collections.Generic;
using UnityStandardAssets._2D;
using UnityEngine;

using Tools.Extensions;
using Tools.AI.NGram;
using Tools.Utility;
using LightJson;
using PCG;

public class GenerateLevelState : BaseState
{
    protected override string DefaultName => "Generate Level State";
    List<List<string>> levelTokens = new List<List<string>>();
    private IGram grammar = null;
    private IGram simpleGrammar = null;
    private int previousIndex = -1;

    public GenerateLevelState(BlackBoard blackBoard) : base(blackBoard) { }

    protected override void OnStateEnter()
    {
        blackBoard.Grid.SetActive(true);

        if (blackBoard.ConfigUI.Config.ProcedurallyGenerateLevels)
        {
            RunProceduralGeneration();
        }
        else
        {
            GenerateInputLevel();
        }

        SetUpEndLevelTiles();
        SetUpPlayer();
        AttachPlayerDiedCallback();

        blackBoard.LevelInfo.Player.gameObject.GetComponent<Platformer2DUserControl>().enabled = false;
        ActivateTrigger(GameTrigger.NextState);
    }

    private void GenerateInputLevel()
    {
        JsonObject info = blackBoard.GameFlow[blackBoard.ProgressIndex].AsJsonObject;
        JsonArray levels = info[FlowKeys.LevelNames].AsJsonArray;
        string levelName = levels[UtilityRandom.Random.Next(levels.Count)];

        blackBoard.LevelInfo = LevelLoader.LoadAndBuild(levelName, blackBoard.Tilemap, blackBoard.CameraFollow);
    }

    private void RunProceduralGeneration()
    {
        if (blackBoard.Reset)
        {
            if (blackBoard.ConfigUI.Config.HeiarchalEnabled)
            {
                blackBoard.DifficultyNGram = NGramFactory.InitHierarchicalNGram(
                    blackBoard.ConfigUI.Config.N,
                    blackBoard.ConfigUI.Config.HeiarchalMemory);
            }
            else
            {
                blackBoard.DifficultyNGram = NGramFactory.InitGrammar(blackBoard.ConfigUI.Config.N);
            }

            GenerateNGram();
            blackBoard.Reset = false;
        }
        else
        {
            if (blackBoard.DifficultyNGram == null)
            {
                if (blackBoard.ConfigUI.Config.HeiarchalEnabled)
                {
                    blackBoard.DifficultyNGram = NGramFactory.InitHierarchicalNGram(
                        blackBoard.ConfigUI.Config.N,
                        blackBoard.ConfigUI.Config.HeiarchalMemory);
                }
                else 
                { 
                    blackBoard.DifficultyNGram = NGramFactory.InitGrammar(blackBoard.ConfigUI.Config.N);
                }
            }

            if (grammar == null || blackBoard.ProgressIndex != previousIndex)
            {
                GenerateNGram();
            }
        }

        if (blackBoard.ConfigUI.Config.DifficultyNGramEnabled)
        {
            grammar.AddGrammar(blackBoard.DifficultyNGram);
        }

        GenerateLevel();
    }

    private void GenerateNGram()
    {
        if (blackBoard.ConfigUI.Config.HeiarchalEnabled)
        {
            grammar = NGramFactory.InitHierarchicalNGram(
                blackBoard.ConfigUI.Config.N,
                blackBoard.ConfigUI.Config.HeiarchalMemory);
        }
        else 
        {
            grammar = NGramFactory.InitGrammar(blackBoard.ConfigUI.Config.N);
        }

        for (int i = 0; i <= blackBoard.ProgressIndex; ++i)
        {
            if (blackBoard.ConfigUI.Config.UsingTieredGeneration || blackBoard.ProgressIndex == i)
            {
                JsonObject tierInfo = blackBoard.GameFlow[i].AsJsonObject;
                JsonArray tierLevels = tierInfo[FlowKeys.LevelNames].AsJsonArray;
                foreach (string levelName in tierLevels)
                {
                    List<string> columns = LevelParser.BreakMapIntoColumns(levelName);
                    columns.RemoveAt(columns.Count - 1); // remove flag at the end
                    List<string> tokens = blackBoard.iDContainer.GetIDs(columns);

                    //Debug.LogWarning("simplified tokens here");
                    //List<string> simplified = LevelParser.BreakColumnsIntoSimplifiedTokens(columns);

                    NGramTrainer.Train(grammar, tokens);
                    levelTokens.Add(tokens);

                    if (blackBoard.ProgressIndex != i)
                    {
                        grammar.UpdateMemory(blackBoard.ConfigUI.Config.TieredGenerationMemoryUpdate);
                        levelTokens.Clear();
                    }
                }
            }
        }
    }

    private void GenerateLevel()
    {
        ICompiledGram compiledGram = grammar.Compile();
        List<string> levelIDs = NGramGenerator.Generate(
            compiledGram,
            levelTokens.RandomValue().GetRange(0, blackBoard.ConfigUI.Config.N + 7),
            blackBoard.ConfigUI.Config.LevelSize);

        List<List<char>> level = new List<List<char>>();
        foreach (string columnID in levelIDs)
        {
            List<char> column = new List<char>();
            string col = blackBoard.iDContainer.GetToken(columnID);

            foreach (char tileCharacter in col)
            {
                column.Add(tileCharacter);
            }

            level.Add(column);
        }

        // add ending column to the level
        char flagChar = Tile.playerOneFinish.ToChar();
        List<char> endingColumn = new List<char>();
        for (int i = 0; i < level[0].Count; ++i)
        {
            endingColumn.Add(flagChar);
        }

        level.Add(endingColumn);

        // set blackboard for level generation
        blackBoard.LevelIds = levelIDs;
        blackBoard.LevelInfo = LevelLoader.Build(level, blackBoard.Tilemap, blackBoard.CameraFollow);
    }

    private void SetUpPlayer()
    {
        blackBoard.LevelInfo.Player.PlayerDiedCallback = PlayerDiedCallback;
    }

    private void SetUpEndLevelTiles()
    {
        foreach (EndLevel endLevelTile in blackBoard.LevelInfo.EndLevelTiles)
        {
            endLevelTile.PlayerWonCallback = () => 
            { 
                ActivateTrigger(GameTrigger.PlayerWon);
            };
        }
    }

    private void AttachPlayerDiedCallback()
    {
        foreach (GameObject turret in blackBoard.LevelInfo.Turrets)
        {
            turret.GetComponent<FireMissile>().HitPlayerCallback = PlayerDiedCallback;
        }

        foreach (GameObject enemy in blackBoard.LevelInfo.Enemies)
        {
            enemy.GetComponent<AttackPlayer>().AddHitPlayerCallback(PlayerDiedCallback);
        }
    }

    private void PlayerDiedCallback()
    {
        ActivateTrigger(GameTrigger.PlayerDied);
    }
}
