using System.Collections.Generic;
using Tools.AI.NGram;
using UnityEngine;

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
        PCG.LevelParser lp = new PCG.LevelParser();
        List<string> levelTokens = lp.GetLevelTokens("level001"); // @TODO: change me

        int size = 3;
        IGram gram = NGramFactory.InitializeGrammar(size);
        NGramTrainer.Train(gram, levelTokens);
        ICompiledGram cGram = gram.Compile();
        List<string> level = NGramGenerator.Generate(
            cGram, 
            levelTokens.GetRange(0, size + 1), 
            10, 
            20);


        //blackBoard.CameraFollow.gameObject.SetActive(true);
    }

    protected override void OnStateExit()
    {
        blackBoard.CameraFollow.gameObject.SetActive(false);
    }
}