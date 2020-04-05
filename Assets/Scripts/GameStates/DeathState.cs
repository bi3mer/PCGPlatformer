public class DeathState : BaseState
{
    protected override string DefaultName => "Death State";
    private bool addedCallback = false;

    public DeathState(BlackBoard blackBoard) : base(blackBoard)
    {

    }

    protected override void OnStateEnter()
    {
        blackBoard.TryLevelAgainButton.transform.parent.gameObject.SetActive(true);

        if (addedCallback == false)
        {
            blackBoard.TryLevelAgainButton.onClick.AddListener(() =>
            {
                ActivateTrigger(GameTrigger.NextState);
            });
        }
    }

    protected override void OnStateExit()
    {
        blackBoard.TryLevelAgainButton.transform.parent.gameObject.SetActive(false);
    }
}
