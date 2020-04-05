﻿using System.Collections.Generic;
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
        int n = info[FlowKeys.N].AsInteger;

        NGramIDContainer idContainer = new NGramIDContainer(idSize: 2);
        List<List<string>> levelTokens = new List<List<string>>();
        IGram gram = NGramFactory.InitializeGrammar(n);

        foreach (JsonValue levelName in levels)
        { 
            List<string> columns = LevelParser.BreakMapIntoColumns(levelName);
            List<string> tokens = idContainer.GetIDs(columns);

            NGramTrainer.Train(gram, tokens);
            levelTokens.Add(tokens);
        }

        ICompiledGram cGram = gram.Compile();
        List<string> levelIDs = NGramGenerator.Generate(
            cGram,
            levelTokens.RandomValue().GetRange(0, n + 1),
            minSize,
            maxSize);

        List<List<string>> level = new List<List<string>>();
        foreach (string columnID in levelIDs)
        {
            List<string> column = new List<string>();
            string col = idContainer.GetToken(columnID);

            foreach (char tileCharacter in col)
            {
                column.Add(tileCharacter.ToString());
            }

            level.Add(column);
        }

        blackBoard.Grid.SetActive(true);
        blackBoard.LevelInfo = LevelLoader.Build(level, blackBoard.Tilemap, blackBoard.CameraFollow);
    }

    private void SetUpPlayer()
    {
        blackBoard.LevelInfo.Player.PlayerDiedCallback = () =>
        {
            SetBool(GameBool.PlayerDied, true);
            ActivateTrigger(GameTrigger.NextState);
        };
    }

    private void SetUpEndLevelTiles()
    {
        foreach (EndLevel el in blackBoard.LevelInfo.EndLevelTiles)
        {
            el.PlayerWonCallback = () =>
            {
                SetBool(GameBool.PlayerDied, false);
                ActivateTrigger(GameTrigger.NextState);
            };
        }
    }
}
