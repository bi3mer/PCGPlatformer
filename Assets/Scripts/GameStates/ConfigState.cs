using Tools.AI.NGram;
using LightJson;

public class ConfigState : BaseState
{
    protected override string DefaultName => "Config State";

    public ConfigState(BlackBoard blackBoard) : base(blackBoard)
    {
    }

    protected override void OnStateEnter()
    {
        JsonObject info = blackBoard.GameFlow[blackBoard.ProgressIndex].AsJsonObject;

        blackBoard.DifficultyMemoryUpdate = (float) info[FlowKeys.DifficultyMemoryUpdate].AsNumber;
        blackBoard.DifficultyNGramActive = info[FlowKeys.DifficultyNGramActive].AsBoolean;
        blackBoard.TieredMemoryUpdate = (float) info[FlowKeys.TieredMemoryUpdate].AsNumber;
        blackBoard.DifficultyRight = info[FlowKeys.DifficultyRight].AsInteger;
        blackBoard.DifficultyLeft = info[FlowKeys.DifficultyLeft].AsInteger;
        blackBoard.Tiered = info[FlowKeys.Tiered].AsBoolean;
        blackBoard.N = info[FlowKeys.N].AsInteger;

        blackBoard.DifficultyNGram = NGramFactory.InitializeGrammar(blackBoard.N); 
        ++blackBoard.ProgressIndex;

        ActivateTrigger(GameTrigger.NextState);
    }

    protected override void OnStateExit()
    {
    }
}
