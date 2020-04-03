using System.Collections.Generic;
using Tools.AI.NGram.Utility;
using Tools.AI.NGram;
using UnityEngine;
using PCG;

public class PlayState : BaseState
{
    protected override string DefaultName => "Game State";
    private PlayLevelData playLevelData = null;

    public PlayState(BlackBoard blackBoard, PlayLevelData playLevelData) : base(blackBoard)
    {
        this.playLevelData = playLevelData;
    }

    protected override void OnStateEnter()
    {
        NGramIDContainer idContainer = new NGramIDContainer(idSize: 5);
        List<string> columns = LevelParser.BreakMapIntoColumns("level001"); // @todo: remove hardcoded string
        List<string> levelTokens = idContainer.GetIDs(columns);

        int size = 3;
        IGram gram = NGramFactory.InitializeGrammar(size);
        NGramTrainer.Train(gram, levelTokens);
        ICompiledGram cGram = gram.Compile();
        List<string> levelIDs = NGramGenerator.Generate(
            cGram, 
            levelTokens.GetRange(0, size + 1), 
            10, 
            20);

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
        LevelLoader.Build(level, playLevelData.Tilemap, blackBoard.CameraFollow);
        blackBoard.CameraFollow.enabled = true;
    }

    protected override void OnStateExit()
    {
        blackBoard.CameraFollow.enabled = false;
        blackBoard.Grid.SetActive(false);
    }
}