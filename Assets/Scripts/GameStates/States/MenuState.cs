public class MenuState : BaseState
{
    protected override string DefaultName => "Menu State";
    private MenuData menuData;
    private bool addedCallback = false;

    public MenuState(BlackBoard blackBoard, MenuData menuData) : base(blackBoard)
    {
        this.menuData = menuData;
    }

    protected override void OnStateEnter()
    {
        menuData.Show();

        if (addedCallback == false)
        {
            menuData.AddStartCallback(() => ActivateTrigger(GameTrigger.NextState));
        }
    }

    protected override void OnStateExit()
    {
        menuData.Hide();
    }
}
