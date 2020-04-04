public class MenuState : BaseState
{
    protected override string DefaultName => "Menu State";
    private bool addedCallback = false;

    public MenuState(BlackBoard blackBoard) : base(blackBoard) { }

    protected override void OnStateEnter()
    {
        blackBoard.StartGameButton.transform.parent.gameObject.SetActive(true);

        if (addedCallback == false)
        {
            blackBoard.StartGameButton.onClick.AddListener(() => ActivateTrigger(GameTrigger.NextState));
        }
    }

    protected override void OnStateExit()
    {
        blackBoard.StartGameButton.transform.parent.gameObject.SetActive(false);
    }
}
