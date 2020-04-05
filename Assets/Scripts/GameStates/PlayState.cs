using UnityStandardAssets._2D;
using UnityEngine;

public class PlayState : BaseState
{
    protected override string DefaultName => "Game State";

    public PlayState(BlackBoard blackBoard) : base(blackBoard) { }

    protected override void OnStateEnter()
    {
        blackBoard.LevelInfo.Player.gameObject.GetComponent<Platformer2DUserControl>().enabled = true;
    }

    protected override void OnStateExit()
    {
        blackBoard.CameraFollow.enabled = false;
        blackBoard.LevelInfo.DestroyGameObjects();
        blackBoard.Tilemap.ClearAllTiles();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag(Tags.Missile))
        {
            MonoBehaviour.Destroy(go);
        }
    }
}