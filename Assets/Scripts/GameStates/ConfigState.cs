public class ConfigState : BaseState
{
    protected override string DefaultName => "Config State";
    private bool addedCallback = false;

    public ConfigState(BlackBoard blackBoard) : base(blackBoard) { }

    protected override void OnStateEnter()
    {
        blackBoard.ConfigGameObject.SetActive(true);

        if (addedCallback == false)
        {
            blackBoard.ConfigBackButton.onClick.AddListener(() =>
            {
                ActivateTrigger(GameTrigger.GotoMainMenu);
            });
        }
    }

    protected override void OnStateExit()
    {
        blackBoard.ConfigGameObject.SetActive(false);
    }
}
