using LightJson;

public class ReadGameFlowState : BaseState
{
    protected override string DefaultName => "Read Game Flow State";

    public ReadGameFlowState(BlackBoard blackBoard) : base(blackBoard) { }

    protected override void OnStateEnter()
    {
        JsonArray flow = blackBoard.GameFlow;
        int index = blackBoard.ProgressIndex;

        if (index >= flow.Count) 
        {
            ActivateTrigger(GameTrigger.GotoGameOver);
        }
        else if (flow[index].AsJsonObject[FlowKeys.Type].AsString.Equals(FlowTypeValues.TypeGame))
        {
            ActivateTrigger(GameTrigger.GotoGame);
        }
        else
        {
            UnityEngine.Debug.Log($"{flow[index].AsJsonObject[FlowKeys.Type].AsString} is unhandled.");
            ActivateTrigger(GameTrigger.GotoGameOver);
        }
    }
}