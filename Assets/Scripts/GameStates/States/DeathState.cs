public class DeathState : BaseState
{
    protected override string DefaultName => "Death State";

    public DeathState(BlackBoard blackBoard) : base(blackBoard)
    {

    }

    protected override void OnStateEnter()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }
}
