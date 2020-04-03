public class PlayState : BaseState
{
    protected override string DefaultName => "Game State";
    private PlayLevelData playLevelData = null;

    public PlayState(BlackBoard blackBoard, PlayLevelData playLevelData) : base(blackBoard)
    {
        this.playLevelData = playLevelData;
    }

    protected override void OnStateEnter()
    {
        blackBoard.CameraFollow.gameObject.SetActive(true);
    }

    protected override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }
}
