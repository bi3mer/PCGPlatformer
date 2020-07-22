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
    private List<List<string>> levelColumns = new List<List<string>>();
    private List<List<string>> simplifiedLevelColumns = new List<List<string>>();

    private IGram grammar = null;
    private IGram simpleGrammar = null;
    private int previousIndex = -1;

    public GenerateLevelState(BlackBoard blackBoard) : base(blackBoard) { }

    protected override void OnStateEnter()
    {
        simplifiedLevelColumns.Clear();
        levelColumns.Clear();
        blackBoard.Grid.SetActive(true);
        bool generationWorked = true;

        if (blackBoard.ConfigUI.Config.ProcedurallyGenerateLevels)
        {
            generationWorked = RunProceduralGeneration();
        }
        else
        {
            GenerateInputLevel();
        }

        if (generationWorked)
        {
            SetUpEndLevelTiles();
            SetUpPlayer();
            AttachPlayerDiedCallback();

            blackBoard.LevelInfo.Player.gameObject.GetComponent<Platformer2DUserControl>().enabled = false;
            ActivateTrigger(GameTrigger.NextState);
        }
        else
        {
            blackBoard.SimpleDifficultyNGram = null;
            blackBoard.DifficultyNGram = null;
            blackBoard.ProgressIndex = 0;
            blackBoard.Reset = true;

            MessagePanel.Instance.Title = "Failed Generation";
            MessagePanel.Instance.Body = "Try to reduce the required level size or make sure to use hierarchical or backoff n-grams.";
            MessagePanel.Instance.Callback = () =>
            {
                ActivateTrigger(GameTrigger.GotoMainMenu);
            };
            MessagePanel.Instance.Active = true;
        }
    }

    private void GenerateInputLevel()
    {
        JsonObject info = blackBoard.GameFlow[blackBoard.ProgressIndex].AsJsonObject;
        JsonArray levels = info[FlowKeys.LevelNames].AsJsonArray;
        string levelName = levels[UtilityRandom.Random.Next(levels.Count)];

        blackBoard.LevelInfo = LevelLoader.LoadAndBuild(levelName, blackBoard.Tilemap, blackBoard.CameraFollow);
    }

    private bool RunProceduralGeneration()
    {
        if (blackBoard.Reset)
        {
            if (blackBoard.ConfigUI.Config.HeiarchalEnabled)
            {
                blackBoard.DifficultyNGram = NGramFactory.InitHierarchicalNGram(
                    blackBoard.ConfigUI.Config.N,
                    blackBoard.ConfigUI.Config.HeiarchalMemory);

                blackBoard.SimpleDifficultyNGram = NGramFactory.InitHierarchicalNGram(
                    blackBoard.ConfigUI.Config.N,
                    blackBoard.ConfigUI.Config.HeiarchalMemory);
            }
            else if (blackBoard.ConfigUI.Config.BackOffEnabled)
            {
                blackBoard.DifficultyNGram = NGramFactory.InitBackOffNGram(
                    blackBoard.ConfigUI.Config.N,
                    blackBoard.ConfigUI.Config.BackOffMemory);

                blackBoard.SimpleDifficultyNGram = NGramFactory.InitBackOffNGram(
                    blackBoard.ConfigUI.Config.N,
                    blackBoard.ConfigUI.Config.BackOffMemory);
            }
            else
            {
                blackBoard.DifficultyNGram = NGramFactory.InitGrammar(blackBoard.ConfigUI.Config.N);
                blackBoard.SimpleDifficultyNGram = NGramFactory.InitGrammar(blackBoard.ConfigUI.Config.N);
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

                    blackBoard.SimpleDifficultyNGram = NGramFactory.InitHierarchicalNGram(
                        blackBoard.ConfigUI.Config.N,
                        blackBoard.ConfigUI.Config.HeiarchalMemory);
                }
                else if (blackBoard.ConfigUI.Config.BackOffEnabled)
                {
                    blackBoard.DifficultyNGram = NGramFactory.InitBackOffNGram(
                        blackBoard.ConfigUI.Config.N,
                        blackBoard.ConfigUI.Config.BackOffMemory);

                    blackBoard.SimpleDifficultyNGram = NGramFactory.InitBackOffNGram(
                        blackBoard.ConfigUI.Config.N,
                        blackBoard.ConfigUI.Config.BackOffMemory);
                }
                else
                {
                    blackBoard.DifficultyNGram = NGramFactory.InitGrammar(blackBoard.ConfigUI.Config.N);
                    blackBoard.SimpleDifficultyNGram = NGramFactory.InitGrammar(blackBoard.ConfigUI.Config.N);
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

        return GenerateLevel();
    }

    private void GenerateNGram()
    {
        if (blackBoard.ConfigUI.Config.HeiarchalEnabled)
        {
            grammar = NGramFactory.InitHierarchicalNGram(
                blackBoard.ConfigUI.Config.N,
                blackBoard.ConfigUI.Config.HeiarchalMemory);

            simpleGrammar = NGramFactory.InitHierarchicalNGram(
                blackBoard.ConfigUI.Config.N,
                blackBoard.ConfigUI.Config.HeiarchalMemory);
        }
        else if (blackBoard.ConfigUI.Config.BackOffEnabled)
        {
            grammar = NGramFactory.InitHierarchicalNGram(
                    blackBoard.ConfigUI.Config.N,
                    blackBoard.ConfigUI.Config.BackOffMemory);

            simpleGrammar = NGramFactory.InitHierarchicalNGram(
                blackBoard.ConfigUI.Config.N,
                blackBoard.ConfigUI.Config.BackOffMemory);
        }
        else
        {
            grammar = NGramFactory.InitGrammar(blackBoard.ConfigUI.Config.N);
            simpleGrammar = NGramFactory.InitGrammar(blackBoard.ConfigUI.Config.N);
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
                    List<string> simpleColumns = LevelParser.BreakColumnsIntoSimplifiedTokens(
                        columns,
                        blackBoard.ConfigUI.Config.Game == Games.Custom);

                    levelColumns.Add(columns);
                    simplifiedLevelColumns.Add(simpleColumns);

                    NGramTrainer.Train(grammar, columns);
                    NGramTrainer.Train(simpleGrammar, simpleColumns);

                    if (blackBoard.ProgressIndex != i)
                    {
                        grammar.UpdateMemory(blackBoard.ConfigUI.Config.TieredGenerationMemoryUpdate);
                        levelColumns.Clear();
                        simplifiedLevelColumns.Clear();
                    }
                }
            }
        }
    }

    private bool GenerateLevel()
    {
        List<List<char>> level = new List<List<char>>();

        if (blackBoard.ConfigUI.Config.UsingSimplifiedNGram)
        {
            ICompiledGram compiledGram = simpleGrammar.Compile();
            int levelIndex = levelColumns.RandomIndex();

            List<string> simpleInput = simplifiedLevelColumns[levelIndex].GetRange
                (0, 
                blackBoard.ConfigUI.Config.N + 7);

            blackBoard.LevelColumns = levelColumns[levelIndex].GetRange(
                0, 
                blackBoard.ConfigUI.Config.N + 7);

            blackBoard.SimpleLevelColumns = NGramGenerator.Generate(
                compiledGram,
                simpleInput,
                blackBoard.ConfigUI.Config.LevelSize);

            compiledGram = grammar.Compile();
            blackBoard.LevelColumns = NGramGenerator.GenerateRestricted(
                compiledGram,
                blackBoard.LevelColumns,
                blackBoard.SimpleLevelColumns,
                (inColumn) =>
                {
                    return LevelParser.ClassifyColumn(
                        inColumn,
                        blackBoard.ConfigUI.Config.Game);
                });
        }
        else 
        {
            ICompiledGram compiledGram = grammar.Compile();
            blackBoard.LevelColumns = NGramGenerator.Generate(
                compiledGram,
                levelColumns.RandomValue().GetRange(0, blackBoard.ConfigUI.Config.N + 7),
                blackBoard.ConfigUI.Config.LevelSize);
        }

        bool generationWorked = blackBoard.LevelColumns != null;
        if (generationWorked)
        {
            foreach (string column in blackBoard.LevelColumns)
            {
                level.Add(new List<char>(column));
            }

            // add ending column to the level
            char flagChar = Tile.playerOneFinish.ToChar();
            List<char> endingColumn = new List<char>();
            for (int i = 0; i < level[0].Count; ++i)
            {
                endingColumn.Add(flagChar);
            }

            level.Add(endingColumn);
            blackBoard.LevelInfo = LevelLoader.Build(level, blackBoard.Tilemap, blackBoard.CameraFollow);
        }

        return generationWorked;
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
