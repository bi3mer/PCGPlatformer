public class LevelBeatenState : BaseState
{
    protected override string DefaultName => "Level Beaten State";

    private bool addedCallbacks = false;

    public LevelBeatenState(BlackBoard blackBoard) : base(blackBoard)
    {

    }

    protected override void OnStateEnter()
    {
        blackBoard.GotoNextLevelButton.transform.parent.gameObject.SetActive(true);

        if (addedCallbacks == false)
        {
            addedCallbacks = true;

            blackBoard.GotoNextLevelButton.onClick.AddListener(() => 
            {
                blackBoard.ProgressIndex += 1;
                ActivateTrigger(GameTrigger.GotoNextLevel);
            });

            blackBoard.PlayLevelAgainButton.onClick.AddListener(() => 
            {
                ActivateTrigger(GameTrigger.ReplayLevel);
            });
        }
    }

    protected override void OnStateExit()
    {
        blackBoard.GotoNextLevelButton.transform.parent.gameObject.SetActive(false);
    }
}
