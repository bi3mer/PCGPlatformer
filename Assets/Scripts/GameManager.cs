using UnityEngine.Assertions;
using UnityEngine;

using Tools.AI.StateMachine;

public enum GameTrigger
{ 
    NextState = 0,
    ReplayLevel,
    GotoSurvey,
    GotoGame
}

public enum GameBool
{ 
    PlayerDied = 0,
}

[RequireComponent(typeof(BlackBoard))]
public class GameManager : MonoBehaviour
{
    private StateMachine<GameBool, GameTrigger> sm = null;
    private BlackBoard blackBoard = null;

    private void Awake()
    {
        blackBoard = GetComponent<BlackBoard>();
        Assert.IsNotNull(blackBoard);

        blackBoard.Grid.SetActive(false);
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
        sm = new StateMachine<GameBool, GameTrigger>(true);

        PostGameSurveyState postGameSurveyState = new PostGameSurveyState(blackBoard);
        GenerateLevelState generateLevelState = new GenerateLevelState(blackBoard);
        ReadGameFlowState readGameFlowState = new ReadGameFlowState(blackBoard);
        LevelBeatenState levelBeatenState = new LevelBeatenState(blackBoard);
        CountDownState countDownState = new CountDownState(blackBoard);
        EndGameState endGameState = new EndGameState(blackBoard);
        SurveyState surveyState = new SurveyState(blackBoard);
        DeathState deathState = new DeathState(blackBoard);
        MenuState menuState = new MenuState(blackBoard);
        PlayState playState = new PlayState(blackBoard);
        EmptyState emptyState = new EmptyState();

        sm.AddEntryState(emptyState);
        sm.AddState(postGameSurveyState);
        sm.AddState(generateLevelState);
        sm.AddState(readGameFlowState);
        sm.AddState(levelBeatenState);
        sm.AddState(countDownState);
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
            readGameFlowState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        sm.AddTransition(
            readGameFlowState,
            generateLevelState,
            sm.CreateTriggerCondition(GameTrigger.GotoGame));

        sm.AddTransition(
            readGameFlowState,
            surveyState,
            sm.CreateTriggerCondition(GameTrigger.GotoSurvey));

        sm.AddTransition(
            generateLevelState,
            countDownState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        sm.AddTransition(
            countDownState,
            playState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        sm.AddTransition(
            playState,
            deathState,
            sm.CreateTriggerCondition(GameTrigger.NextState),
            sm.CreateBoolCondition(GameBool.PlayerDied, true));

        sm.AddTransition(
            playState,
            levelBeatenState,
            sm.CreateTriggerCondition(GameTrigger.NextState),
            sm.CreateBoolCondition(GameBool.PlayerDied, false));

        sm.AddTransition(
            deathState,
            generateLevelState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        sm.AddTransition(
            levelBeatenState,
            generateLevelState,
            sm.CreateTriggerCondition(GameTrigger.ReplayLevel));

        sm.AddTransition(
            levelBeatenState,
            readGameFlowState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        //sm.AddTransition(
        //    levelBeatenState,
        //    null,
        //    sm.CreateTriggerCondition(GameTrigger.GotoNextLevel));
    }
}
