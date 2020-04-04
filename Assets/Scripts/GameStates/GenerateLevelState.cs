using System.Collections.Generic;
using Tools.AI.NGram.Utility;
using Tools.AI.NGram;
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
        UnityEngine.Debug.LogWarning("using hardcoded level string");
        UnityEngine.Debug.LogWarning("using hardocded min size");
        UnityEngine.Debug.LogWarning("using hardocded max size");
        UnityEngine.Debug.LogWarning("using hardocded n");

        NGramIDContainer idContainer = new NGramIDContainer(idSize: 2);
        List<string> columns = LevelParser.BreakMapIntoColumns("level001");
        List<string> levelTokens = idContainer.GetIDs(columns);

        int size = 3;
        IGram gram = NGramFactory.InitializeGrammar(size);
        NGramTrainer.Train(gram, levelTokens);
        ICompiledGram cGram = gram.Compile();
        List<string> levelIDs = NGramGenerator.Generate(
            cGram,
            levelTokens.GetRange(0, size + 1),
            50,
            100);

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
