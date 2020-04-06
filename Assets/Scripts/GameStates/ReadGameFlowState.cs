using LightJson;

public class ReadGameFlowState : BaseState
{
    protected override string DefaultName => "Read Game Flow State";

    public ReadGameFlowState(BlackBoard blackBoard) : base(blackBoard)
    {

    }

    protected override void OnStateEnter()
    {
        JsonArray flow = blackBoard.GameFlow;
        int index = blackBoard.ProgressIndex;

        if (flow[index].AsJsonObject[FlowKeys.Type].AsString.Equals(FlowTypeValues.TypeGame))
        {
            ActivateTrigger(GameTrigger.GotoGame);
        }
        else if (flow[index].AsJsonObject[FlowKeys.Type].AsString.Equals(FlowTypeValues.TypeConfig))
        {
            ActivateTrigger(GameTrigger.SetUpConfig);
        }
        else
        {
            ActivateTrigger(GameTrigger.GotoSurvey);
        }
    }

    protected override void OnStateExit()
    {
    }
}
