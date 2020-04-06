using UnityStandardAssets._2D;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

using Tools.AI.NGram;
using LightJson;

public class BlackBoard : MonoBehaviour
{
    [Header("Play State")]
    public Button TryLevelAgainButton = null;
    public Button StartGameButton = null;

    [Header("Beat Level State")]
    public Button PlayLevelAgainButton = null;
    public Button GotoNextLevelButton = null;

    [Header("Count Down State")]
    public TextMeshProUGUI CountDownText = null;

    [Header("Survey State")]
    public GameObject Survey = null;
    public GameObject SurveyContentSection = null;

    [Header("General Game")]
    public Camera2DFollow CameraFollow = null;
    public GameObject Grid = null;
    public Tilemap Tilemap = null;

    public string GameFlowName = null;
    public JsonArray GameFlow = null;
    public int ProgressIndex = 0;

    [Range(0f, 1f)]
    public float TieredMemoryUpdate = 0.8f;

    public LevelInfo LevelInfo = null;
    public NGram DifficultyNGram;

    [HideInInspector]
    public bool Tiered = false;

    [HideInInspector]
    public int N = 3;

    private void Awake()
    {
        Assert.IsNotNull(CameraFollow);
        Assert.IsNotNull(TryLevelAgainButton);
        Assert.IsNotNull(StartGameButton);

        Assert.IsNotNull(PlayLevelAgainButton);
        Assert.IsNotNull(GotoNextLevelButton);

        Assert.IsNotNull(CountDownText);

        Assert.IsNotNull(Survey);
        Assert.IsNotNull(SurveyContentSection);

        Assert.IsNotNull(Tilemap);
        Assert.IsNotNull(Grid);

        Assert.IsFalse(string.IsNullOrEmpty(GameFlowName));
        TextAsset text = Resources.Load<TextAsset>($"GameFlow/{GameFlowName}");
        GameFlow = JsonValue.Parse(text.text).AsJsonArray;
    }
}
