public class ConfigState : BaseState
{
    protected override string DefaultName => "Config State";
    private bool addedCallback = false;

    public ConfigState(BlackBoard blackBoard) : base(blackBoard) { }

    protected override void OnStateEnter()
    {
        blackBoard.ConfigUI.gameObject.SetActive(true);

        if (addedCallback == false)
        {
            blackBoard.ConfigUI.BackButton.onClick.AddListener(() =>
            {
                ActivateTrigger(GameTrigger.GotoMainMenu);
            });
        }
    }

    protected override void OnStateExit()
    {
        blackBoard.ConfigUI.gameObject.SetActive(false);
    }
}
