public class GameOverState : BaseState
{
    protected override string DefaultName => "Game Over State";
    private bool addedCallback = false;

    public GameOverState(BlackBoard blackBoard) : base(blackBoard) { }

    protected override void OnStateEnter()
    {
        blackBoard.GotoMainMenu.transform.parent.gameObject.SetActive(true);
        blackBoard.ProgressIndex = 0;
        blackBoard.Reset = true;

        if (addedCallback == false)
        {
            blackBoard.GotoMainMenu.onClick.AddListener(() => ActivateTrigger(GameTrigger.GotoMainMenu));
            addedCallback = true;
        }
    }

    protected override void OnStateExit()
    {
        blackBoard.GotoMainMenu.transform.parent.gameObject.SetActive(false);
    }
}
