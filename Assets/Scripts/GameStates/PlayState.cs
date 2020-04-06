using System.Collections.Generic;
using UnityStandardAssets._2D;
using UnityEngine.Tilemaps;
using UnityEngine;

using Tools.Utility;
using Tools.AI.NGram;

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
        if (blackBoard.DifficultyNGramActive)
        { 
            UpdateDifficultyNGram();
        }

        blackBoard.CameraFollow.enabled = false;
        blackBoard.LevelInfo.DestroyGameObjects();
        blackBoard.Tilemap.ClearAllTiles();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag(Tags.Missile))
        {
            MonoBehaviour.Destroy(go);
        }
    }

    private void UpdateDifficultyNGram()
    {
        Vector3 playerPosition = blackBoard.LevelInfo.Player.transform.position;
        int toRight = blackBoard.DifficultyRight;
        int toLeft = blackBoard.DifficultyLeft;
        Tilemap tilemap = blackBoard.Tilemap;

        Vector3Int tilePosition = tilemap.WorldToCell(playerPosition);

        int x = Math.Max(tilePosition.x - toLeft, tilemap.cellBounds.xMin);
        int xMax = Math.Min(tilePosition.x + toRight, tilemap.cellBounds.xMax - 1);

        blackBoard.DifficultyNGram.UpdateMemory(blackBoard.DifficultyMemoryUpdate);
        NGramTrainer.Train(blackBoard.DifficultyNGram, blackBoard.LevelIds.GetRange(x, xMax - x + 1));
    }
}