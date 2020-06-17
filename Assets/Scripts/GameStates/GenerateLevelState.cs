using System.Collections.Generic;
using UnityStandardAssets._2D;
using UnityEngine;

using Tools.Extensions;
using Tools.AI.NGram;
using LightJson;
using PCG;

public class GenerateLevelState : BaseState
{
    protected override string DefaultName => "Generate Level State";
    List<List<string>> levelTokens = new List<List<string>>();
    private IGram grammar = null;
    private int previousIndex = -1;

    public GenerateLevelState(BlackBoard blackBoard) : base(blackBoard) { }

    protected override void OnStateEnter()
    {
        if (blackBoard.Reset)
        {
            blackBoard.DifficultyNGram = NGramFactory.InitializeGrammar(blackBoard.ConfigUI.Config.N);
            GenerateNGram();

            blackBoard.Reset = false;
        }
        else
        {
            if (blackBoard.DifficultyNGram == null)
            {
                blackBoard.DifficultyNGram = NGramFactory.InitializeGrammar(blackBoard.ConfigUI.Config.N);
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
        SetUpEndLevelTiles();
        SetUpPlayer();
        AttachPlayerDiedCallback();

        blackBoard.LevelInfo.Player.gameObject.GetComponent<Platformer2DUserControl>().enabled = false;

        ActivateTrigger(GameTrigger.NextState);
    }

    private void GenerateNGram()
    {
        JsonObject info = blackBoard.GameFlow[blackBoard.ProgressIndex].AsJsonObject;
        JsonArray levels = info[FlowKeys.LevelNames].AsJsonArray;

        grammar = NGramFactory.InitializeGrammar(blackBoard.ConfigUI.Config.N);

        if (blackBoard.ConfigUI.Config.UsingTieredGeneration)
        {
            for (int i = 0; i < blackBoard.ProgressIndex; ++i)
            {
                JsonObject tierInfo = blackBoard.GameFlow[i].AsJsonObject;
                JsonArray tierLevels = tierInfo[FlowKeys.LevelNames].AsJsonArray;
                foreach (string levelName in tierLevels)
                {
                    List<string> columns = LevelParser.BreakMapIntoColumns(levelName);
                    List<string> tokens = blackBoard.iDContainer.GetIDs(columns);

                    NGramTrainer.Train(grammar, tokens);
                    levelTokens.Add(tokens);

                    grammar.UpdateMemory(blackBoard.ConfigUI.Config.TieredGenerationMemoryUpdate);
                }

                levelTokens.Clear();
            }
        }

        foreach (JsonValue levelName in levels)
        {
            List<string> columns = LevelParser.BreakMapIntoColumns(levelName);
            List<string> tokens = blackBoard.iDContainer.GetIDs(columns);

            NGramTrainer.Train(grammar, tokens);
            levelTokens.Add(tokens);
        }
    }

    private void GenerateLevel()
    {
        int minSize = blackBoard.ConfigUI.Config.MinLevelSize;
        int maxSize = blackBoard.ConfigUI.Config.MaxLevelSize;

        ICompiledGram cGram = grammar.Compile();
        List<string> levelIDs = NGramGenerator.Generate(
            cGram,
            levelTokens.RandomValue().GetRange(0, blackBoard.ConfigUI.Config.N + 4),
            minSize,
            maxSize);

        List<List<string>> level = new List<List<string>>();
        foreach (string columnID in levelIDs)
        {
            List<string> column = new List<string>();
            string col = blackBoard.iDContainer.GetToken(columnID);

            foreach (char tileCharacter in col)
            {
                column.Add(tileCharacter.ToString());
            }

            level.Add(column);
        }

        blackBoard.LevelIds = levelIDs;
        blackBoard.Grid.SetActive(true);
        blackBoard.LevelInfo = LevelLoader.Build(level, blackBoard.Tilemap, blackBoard.CameraFollow);
    }

    private void SetUpPlayer()
    {
        blackBoard.LevelInfo.Player.PlayerDiedCallback = PlayerDiedCallback;
    }

    private void SetUpEndLevelTiles()
    {
        foreach (EndLevel el in blackBoard.LevelInfo.EndLevelTiles)
        {
            el.PlayerWonCallback = () => 
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
