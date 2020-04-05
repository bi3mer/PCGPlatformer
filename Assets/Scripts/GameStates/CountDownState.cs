using UnityEngine;

public class CountDownState : BaseState
{
    protected override string DefaultName => "Count Down State";
    private float timer;

    public CountDownState(BlackBoard blackBoard) : base(blackBoard)
    {

    }

    protected override void OnStateEnter()
    {
        blackBoard.CountDownText.transform.parent.gameObject.SetActive(true);
        blackBoard.CameraFollow.enabled = true;
        timer = 0f;
    }

    protected override void OnStateExit()
    {
        blackBoard.CountDownText.transform.parent.gameObject.SetActive(false);
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer > 3f)
        {
            ActivateTrigger(GameTrigger.NextState);
        }
        else
        {
            //blackBoard.CountDownText.text = $"{timer}";
            blackBoard.CountDownText.text = $"{Mathf.CeilToInt(3f - timer)}";
        }
    }
}
