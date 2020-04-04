using UnityEngine;

public class PlayState : BaseState
{
    protected override string DefaultName => "Game State";

    public PlayState(BlackBoard blackBoard) : base(blackBoard) { }

    protected override void OnStateEnter()
    {
        blackBoard.CameraFollow.enabled = true;
    }

    protected override void OnStateExit()
    {
        blackBoard.CameraFollow.enabled = false;
        blackBoard.LevelInfo.DestroyGameObjects();
    }
}