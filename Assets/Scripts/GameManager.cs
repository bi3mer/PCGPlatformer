using UnityEngine.Assertions;
using UnityEngine;

using Tools.AI.StateMachine;

public enum GameTrigger
{ 
    NextState = 0,
    ReplayLevel,
    SetUpConfig,
    GotoGame,
    PlayerDied,
    PlayerWon,
    GotoMainMenu,
    GotoGameOver,
    GotoConfig
}

public enum GameBool
{ 
    HasSeenInstructions = 0
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
        sm.SetBoolImmediate(
            GameBool.HasSeenInstructions, 
            PlayerPrefs.HasKey(PlayerPrefKeys.HasSeenInstructions));

        sm.ActivateTriggerDeferred(GameTrigger.NextState);
    }

    private void Update()
    {
        sm.Update();
    }

    private void ConstructStateMachine()
    {
        sm = new StateMachine<GameBool, GameTrigger>(verbose: true);

        GenerateLevelState generateLevelState = new GenerateLevelState(blackBoard);
        ReadGameFlowState readGameFlowState = new ReadGameFlowState(blackBoard);
        InstructionState instructionState = new InstructionState(blackBoard);
        LevelBeatenState levelBeatenState = new LevelBeatenState(blackBoard);
        CountDownState countDownState = new CountDownState(blackBoard);
        GameOverState gameOverState = new GameOverState(blackBoard);
        EndGameState endGameState = new EndGameState(blackBoard);
        LoadingState loadingState = new LoadingState(blackBoard);
        ConfigState configState = new ConfigState(blackBoard);
        DeathState deathState = new DeathState(blackBoard);
        MenuState menuState = new MenuState(blackBoard);
        PlayState playState = new PlayState(blackBoard);
        EmptyState emptyState = new EmptyState();

        sm.AddEntryState(emptyState);
        sm.AddState(generateLevelState);
        sm.AddState(readGameFlowState);
        sm.AddState(instructionState);
        sm.AddState(levelBeatenState);
        sm.AddState(countDownState);
        sm.AddState(gameOverState);
        sm.AddState(endGameState);
        sm.AddState(loadingState);
        sm.AddState(configState);
        sm.AddState(deathState);
        sm.AddState(menuState);
        sm.AddState(playState);

        // start by going to the loading
        sm.AddTransition(
            emptyState,
            loadingState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        // loading state always goes to the menu state
        sm.AddTransition(
            loadingState,
            menuState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        // menu to config
        sm.AddTransition(
            menuState,
            configState,
            sm.CreateTriggerCondition(GameTrigger.GotoConfig));

        // config back to menu
        sm.AddTransition(
            configState,
            menuState,
            sm.CreateTriggerCondition(GameTrigger.GotoMainMenu));

        // menu straight to game if the player has already seen the instructions
        sm.AddTransition(
            menuState,
            readGameFlowState,
            sm.CreateTriggerCondition(GameTrigger.GotoGame),
            sm.CreateBoolCondition(GameBool.HasSeenInstructions, true));

        // menu to instructions
        sm.AddTransition(
            menuState,
            instructionState,
            sm.CreateTriggerCondition(GameTrigger.GotoGame),
            sm.CreateBoolCondition(GameBool.HasSeenInstructions, false));

        // instruction to start game state
        sm.AddTransition(
            instructionState,
            readGameFlowState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        // reading game to config to set up variables
        sm.AddTransition(
            readGameFlowState,
            configState,
            sm.CreateTriggerCondition(GameTrigger.SetUpConfig));

        // config back to read game flow
        sm.AddTransition(
            configState,
            readGameFlowState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        // reading game to generating a level
        sm.AddTransition(
            readGameFlowState,
            generateLevelState,
            sm.CreateTriggerCondition(GameTrigger.GotoGame));

        // generating game to countdown 
        sm.AddTransition(
            generateLevelState,
            countDownState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        // countdown to play state
        sm.AddTransition(
            countDownState,
            playState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        // play state to death
        sm.AddTransition(
            playState,
            deathState,
            sm.CreateTriggerCondition(GameTrigger.PlayerDied));

        // death back to generating level
        sm.AddTransition(
            deathState,
            generateLevelState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        // play to level beaten
        sm.AddTransition(
            playState,
            levelBeatenState,
            sm.CreateTriggerCondition(GameTrigger.PlayerWon));

        // level beating to replay
        sm.AddTransition(
            levelBeatenState,
            generateLevelState,
            sm.CreateTriggerCondition(GameTrigger.ReplayLevel));

        // level beaten back to read game flow to figure out what is next
        sm.AddTransition(
            levelBeatenState,
            readGameFlowState,
            sm.CreateTriggerCondition(GameTrigger.NextState));

        // game is over since read game flow state can't find anything else
        sm.AddTransition(
            readGameFlowState,
            gameOverState,
            sm.CreateTriggerCondition(GameTrigger.GotoGameOver));

        // in the game over state, the only option is to go back to the main menu
        sm.AddTransition(
            gameOverState,
            menuState,
            sm.CreateTriggerCondition(GameTrigger.GotoMainMenu));
    }
}
