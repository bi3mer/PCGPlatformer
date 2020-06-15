using System.Collections.Generic;
using UnityStandardAssets._2D;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

using Tools.AI.NGram.Utility;
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
    
    [Header("Instruction State")]
    public Button InstructionStartGame = null;

    [Header("All Levels Beaten")]
    public Button GotoMainMenu = null;

    [Header("General Game")]
    public Camera2DFollow CameraFollow = null;
    public GameObject Grid = null;
    public Tilemap Tilemap = null;

    public string GameFlowName = null;
    public JsonArray GameFlow = null;
    public int ProgressIndex = 0;


    public NGramIDContainer iDContainer = new NGramIDContainer(idSize: 2);
    public LevelInfo LevelInfo = null;
    public IGram DifficultyNGram;

    [HideInInspector]
    public float TieredMemoryUpdate = 0.9f;

    [HideInInspector]
    public float DifficultyMemoryUpdate = 0.9f;

    [HideInInspector]
    public bool Tiered = false;

    [HideInInspector]
    public int N = 3;

    [HideInInspector]
    public bool DifficultyNGramActive = false;

    [HideInInspector]
    public int DifficultyLeft = 0;

    [HideInInspector]
    public int DifficultyRight = 0;

    [HideInInspector]
    public List<string> LevelIds;

    private void Awake()
    {
        Assert.IsNotNull(CameraFollow);
        Assert.IsNotNull(TryLevelAgainButton);
        Assert.IsNotNull(StartGameButton);

        Assert.IsNotNull(PlayLevelAgainButton);
        Assert.IsNotNull(GotoNextLevelButton);

        Assert.IsNotNull(CountDownText);

        Assert.IsNotNull(InstructionStartGame);

        Assert.IsNotNull(GotoMainMenu);

        Assert.IsNotNull(Tilemap);
        Assert.IsNotNull(Grid);

        Assert.IsFalse(string.IsNullOrEmpty(GameFlowName));
        TextAsset text = Resources.Load<TextAsset>($"GameFlow/{GameFlowName}");
        GameFlow = JsonValue.Parse(text.text).AsJsonArray;
    }
}
