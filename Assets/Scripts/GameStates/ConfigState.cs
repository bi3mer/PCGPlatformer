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
        blackBoard.Tiered = info[FlowKeys.Tiered].AsBoolean;
        blackBoard.N = info[FlowKeys.N].AsInteger;
        ++blackBoard.ProgressIndex;

        ActivateTrigger(GameTrigger.NextState);
    }

    protected override void OnStateExit()
    {
    }
}
