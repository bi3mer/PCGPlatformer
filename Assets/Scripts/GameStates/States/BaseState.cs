using AITools.StateMachine;

public abstract class BaseState : CoordinatingState<GameBool, GameTrigger>
{
    protected BlackBoard blackBoard;

    public BaseState(BlackBoard blackBoard)
    {
        this.blackBoard = blackBoard;
    }

    public BaseState(BlackBoard blackBoard, string name) : base(name)
    {
        this.blackBoard = blackBoard;
    }
}