using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UpdateSliderText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text = null;

    [SerializeField]
    private string start = "";

    private Slider slider = null;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        Assert.IsNotNull(slider);
        Assert.IsNotNull(text);
    }

    private void Start()
    {
        slider.onValueChanged.AddListener((float val) =>
        {
            if (slider.wholeNumbers)
            {
                text.text = $"{start}{(int) val}";
            }
            else
            { 
                text.text = $"{start}{val}";
            }
        });
    }
}
