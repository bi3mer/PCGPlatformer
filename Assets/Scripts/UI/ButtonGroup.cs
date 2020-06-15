using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

public class ButtonGroup : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons = null;

    private int activeIndex = 0;
    private bool addedCallbacks = false;

    private void Awake()
    {
        Assert.IsNotNull(buttons);
        foreach (Button btn in buttons)
        {
            Assert.IsNotNull(btn);
        }
    }

    private void Start()
    {
        if (addedCallbacks == false)
        {
            for (int i = 0; i < buttons.Length; ++i)
            {
                int copy = i;
                buttons[i].onClick.AddListener(() => UpdateButtonsValues(copy));

                if (i > 0)
                {
                    UpdateButtonNormalColor(i, Color.gray);
                }
            }
        }
    }

    private void UpdateButtonsValues(int index)
    {
        UpdateButtonNormalColor(activeIndex, Color.gray);
        activeIndex = index;
        UpdateButtonNormalColor(activeIndex, Color.white);
    }

    private void UpdateButtonNormalColor(int index, Color color)
    {
        ColorBlock colors = buttons[index].colors;
        colors.normalColor = color;
        buttons[index].colors = colors;
    }
}
