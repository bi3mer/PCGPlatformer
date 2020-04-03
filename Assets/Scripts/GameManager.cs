using UnityEngine.Assertions;
using UnityEngine;

using UnityStandardAssets._2D;
using AITools.StateMachine;

public enum GameTrigger
{ 
    NextState = 0
}

public enum GameBool
{ 

}

public class GameManager : MonoBehaviour
{
    private StateMachine<GameBool, GameTrigger> sm = null;

    [SerializeField]
    private GameObject grid = null;

    [SerializeField]
    private Camera2DFollow cameraFollow = null;

    [SerializeField]
    private MenuData menuData = null;

    [SerializeField]
    private PlayLevelData playLevelData = null;

    private void Awake()
    {
        Assert.IsNotNull(grid);
        Assert.IsNotNull(menuData);
        Assert.IsNotNull(cameraFollow);
        Assert.IsNotNull(playLevelData);

        grid.SetActive(false);

        ConstructStateMachine();
    }

    private void Start()
    {
        sm.Start();
        sm.ActivateTriggerDeferred(GameTrigger.NextState);
    }

    private void Update()
    {
        sm.Update();
    }

    private void ConstructStateMachine()
    {
        BlackBoard blackBoard = new BlackBoard();
        sm = new StateMachine<GameBool, GameTrigger>();

        PostGameSurveyState postGameSurveyState = new PostGameSurveyState(blackBoard);
        EndGameState endGameState = new EndGameState(blackBoard);
        SurveyState surveyState = new SurveyState(blackBoard);
        DeathState deathState = new DeathState(blackBoard);
        MenuState menuState = new MenuState(blackBoard, menuData);
        PlayState playState = new PlayState(blackBoard, playLevelData);
        EmptyState emptyState = new EmptyState();

        sm.AddEntryState(emptyState);
        sm.AddState(postGameSurveyState);
        sm.AddState(endGameState);
        sm.AddState(surveyState);
        sm.AddState(deathState);
        sm.AddState(menuState);
        sm.AddState(playState);

        sm.AddTransition(
            emptyState,
            menuState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        sm.AddTransition(
            menuState,
            playState,
            sm.CreateTriggerCondition(GameTrigger.NextState));
    }
}
