using UnityEngine;

public class InstructionState : BaseState
{
    protected override string DefaultName => "Instruction State";
    private float time;
    private bool addedCallback = false;

    public InstructionState(BlackBoard blackBoard) : base(blackBoard)
    {

    }

    protected override void OnStateEnter()
    {
        time = 0f;
        blackBoard.InstructionStartGame.transform.parent.gameObject.SetActive(true);
        blackBoard.InstructionStartGame.gameObject.SetActive(false);

        if (addedCallback == false)
        {
            blackBoard.InstructionStartGame.onClick.AddListener(() => 
            { 
                ActivateTrigger(GameTrigger.NextState);
            });
        }
    }

    protected override void OnStateExit()
    {
        blackBoard.InstructionStartGame.transform.parent.gameObject.SetActive(false);
    }

    public override void Update()
    {
        time += Time.deltaTime;

        if (time > 3.0)
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.HasSeenInstructions, 1);
            blackBoard.InstructionStartGame.gameObject.SetActive(true);
        }
    }
}
