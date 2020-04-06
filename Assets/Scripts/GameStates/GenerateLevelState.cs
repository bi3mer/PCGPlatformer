using System.Collections.Generic;
using UnityStandardAssets._2D;
using UnityEngine;

using Tools.AI.NGram.Utility;
using Tools.Extensions;
using Tools.AI.NGram;
using LightJson;
using PCG;

public class GenerateLevelState : BaseState
{
    protected override string DefaultName => "Generate Level State";

    public GenerateLevelState(BlackBoard blackBoard) : base(blackBoard)
    {

    }

    protected override void OnStateEnter()
    {
        GenerateLevel();
        SetUpEndLevelTiles();
        SetUpPlayer();
        AttachPlayerDiedCallback();

        blackBoard.LevelInfo.Player.gameObject.GetComponent<Platformer2DUserControl>().enabled = false;

        ActivateTrigger(GameTrigger.NextState);
    }

    protected override void OnStateExit()
    {

    }

    private void GenerateLevel()
    {
        JsonObject info = blackBoard.GameFlow[blackBoard.ProgressIndex].AsJsonObject;
        JsonArray levels = info[FlowKeys.LevelNames].AsJsonArray;
        int minSize = info[FlowKeys.MinSize].AsInteger;
        int maxSize = info[FlowKeys.MaxSize].AsInteger;

        List<List<string>> levelTokens = new List<List<string>>();
        IGram gram = NGramFactory.InitializeGrammar(blackBoard.N);

        if (blackBoard.Tiered)
        {
            for (int i = 0; i < blackBoard.ProgressIndex; ++i)
            {
                JsonObject tierInfo = blackBoard.GameFlow[i].AsJsonObject;

                if (tierInfo[FlowKeys.Type].AsString.Equals(FlowTypeValues.TypeGame))
                {
                    JsonArray tierLevels = tierInfo[FlowKeys.LevelNames].AsJsonArray;
                    foreach (string levelName in tierLevels)
                    {
                        List<string> columns = LevelParser.BreakMapIntoColumns(levelName);
                        List<string> tokens = blackBoard.iDContainer.GetIDs(columns);

                        NGramTrainer.Train(gram, tokens);
                        levelTokens.Add(tokens);

                        gram.UpdateMemory(blackBoard.TieredMemoryUpdate);
                    }

                    levelTokens.Clear();
                }
            }
        }

        if (blackBoard.DifficultyNGramActive)
        {
            gram.AddGrammar(blackBoard.DifficultyNGram);
        }

        foreach (JsonValue levelName in levels)
        { 
            List<string> columns = LevelParser.BreakMapIntoColumns(levelName);
            List<string> tokens = blackBoard.iDContainer.GetIDs(columns);

            NGramTrainer.Train(gram, tokens);
            levelTokens.Add(tokens);
        }

        ICompiledGram cGram = gram.Compile();
        List<string> levelIDs = NGramGenerator.Generate(
            cGram,
            levelTokens.RandomValue().GetRange(0, blackBoard.N + 4),
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
