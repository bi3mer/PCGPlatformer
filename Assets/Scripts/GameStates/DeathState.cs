public class DeathState : BaseState
{
    protected override string DefaultName => "Death State";
    private bool addedCallback = false;

    public DeathState(BlackBoard blackBoard) : base(blackBoard)
    {

    }

    protected override void OnStateEnter()
    {
        blackBoard.DeathMenu.gameObject.SetActive(true);

        if (addedCallback == false)
        {
            addedCallback = true;

            blackBoard.DeathMenu.ReplayLevelButton.onClick.AddListener(() =>
            {
                ActivateTrigger(GameTrigger.NextState);
            });

            blackBoard.DeathMenu.GotoMainMenuButton.onClick.AddListener(() => 
            {
                blackBoard.ProgressIndex = 0;
                blackBoard.Reset = true;
                ActivateTrigger(GameTrigger.GotoMainMenu);
            });
        }
    }

    protected override void OnStateExit()
    {
        blackBoard.DeathMenu.gameObject.SetActive(false);
    }
}
