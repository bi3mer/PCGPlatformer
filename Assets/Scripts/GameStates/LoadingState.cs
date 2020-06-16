public class LoadingState : BaseState
{
    protected override string DefaultName => "LevelLoading State";

    public LoadingState(BlackBoard blackBoard) : base(blackBoard) { }

    protected override void OnStateEnter()
    {
        blackBoard.LoadingScreen.SetActive(true);
        blackBoard.ConfigUI.gameObject.SetActive(true);
    }

    protected override void OnStateExit()
    {
        blackBoard.ConfigUI.gameObject.SetActive(false);
        blackBoard.LoadingScreen.SetActive(false);
    }

    public override void Update()
    {
        if (blackBoard.ConfigUI.StartCalled)
        { 
            ActivateTrigger(GameTrigger.NextState);
        }
    }
}
