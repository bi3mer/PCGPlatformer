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
    [Header("Main Menu")]
    public Button StartGameButton = null;
    public Button ConfigButton = null;

    [Header("Config")]
    public GameObject ConfigGameObject = null;
    public Button ConfigBackButton = null;
    public ConfigUI ConfigUI = null;

    [Header("Play State")]
    public Button TryLevelAgainButton = null;

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

    public int ProgressIndex = 0;

    [Header("Game Flows")]
    public TextAsset PCGPlatformer = null;
    public TextAsset SuperMarioLand = null;
    public TextAsset SuperMarioBros = null;
    public TextAsset SuperMarioBros2 = null;
    public TextAsset SuperMarioBros2Japan = null;

    public NGramIDContainer iDContainer = new NGramIDContainer(idSize: 5);
    public LevelInfo LevelInfo = null;
    public IGram DifficultyNGram;
    public JsonArray ActiveGameFlow = null;

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
        Assert.IsNotNull(StartGameButton);
        Assert.IsNotNull(ConfigButton);

        Assert.IsNotNull(ConfigGameObject);
        Assert.IsNotNull(ConfigBackButton);
        Assert.IsNotNull(ConfigUI);

        Assert.IsNotNull(CameraFollow);
        Assert.IsNotNull(TryLevelAgainButton);

        Assert.IsNotNull(PlayLevelAgainButton);
        Assert.IsNotNull(GotoNextLevelButton);

        Assert.IsNotNull(CountDownText);

        Assert.IsNotNull(InstructionStartGame);

        Assert.IsNotNull(GotoMainMenu);

        Assert.IsNotNull(Tilemap);
        Assert.IsNotNull(Grid);

        Assert.IsNotNull(PCGPlatformer);
        Assert.IsNotNull(SuperMarioLand);
        Assert.IsNotNull(SuperMarioBros);
        Assert.IsNotNull(SuperMarioBros2);
        Assert.IsNotNull(SuperMarioBros2Japan);
    }
}
