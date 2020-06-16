public class LevelBeatenState : BaseState
{
    protected override string DefaultName => "Level Beaten State";

    private bool addedCallbacks = false;

    public LevelBeatenState(BlackBoard blackBoard) : base(blackBoard) { }

    protected override void OnStateEnter()
    {
        blackBoard.LevelBeatenMenu.gameObject.SetActive(true);

        if (addedCallbacks == false)
        {
            addedCallbacks = true;

            blackBoard.LevelBeatenMenu.GotoNextLevelButton.onClick.AddListener(() => 
            {
                blackBoard.ProgressIndex += 1;
                ActivateTrigger(GameTrigger.NextState);
            });

            blackBoard.LevelBeatenMenu.ReplayLevelButton.onClick.AddListener(() => 
            {
                ActivateTrigger(GameTrigger.ReplayLevel);
            });

            blackBoard.LevelBeatenMenu.GotoMainMenuButton.onClick.AddListener(() => 
            {
                ActivateTrigger(GameTrigger.GotoMainMenu);
                blackBoard.ProgressIndex = 0;
                blackBoard.Reset = true;
            });
        }
    }

    protected override void OnStateExit()
    {
        blackBoard.LevelBeatenMenu.gameObject.SetActive(false);
    }
}
