using UnityEngine.UI;
using UnityEngine;

public class SurveyState : BaseState
{
    protected override string DefaultName => "Survey State";

    public SurveyState(BlackBoard blackBoard) : base(blackBoard)
    {

    }

    protected override void OnStateEnter()
    {
        Debug.LogWarning("not using blackboard for building questions");
        Debug.LogWarning("need to add checker to make sure all questions are answered.");
        Debug.LogWarning("need to add button to move to the next state that calls checker first.");

        blackBoard.Survey.SetActive(true);
    }

    protected override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }
}
