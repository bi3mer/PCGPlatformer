using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

public class ConfigUI : MonoBehaviour
{
    [SerializeField]
    private Toggle procedurallyGenerateLevels = null;

    [SerializeField]
    private Button backButton = null;

    [Header("Game Buttons")]
    [SerializeField]
    private Button custom = null;

    [SerializeField]
    private Button superMarioBros = null;

    [SerializeField]
    private Button superMarioBros2 = null;

    [SerializeField]
    private Button superMarioBros2Japan = null;

    [SerializeField]
    private Button superMarioLand = null;

    [Header("Configuration Variables")]
    [SerializeField]
    private Slider nLevel = null;

    [SerializeField]
    private Toggle simplifiedNGramEnabled = null;

    [SerializeField]
    private Toggle heiarchalNGramEanbled = null;

    [SerializeField]
    private Slider heiarchalMemory = null;

    [Header("Level Sizes")]
    [SerializeField]
    private Slider minLevelSize = null;

    [SerializeField]
    private Slider maxLevelSize = null;

    [Header("Tiered Generation")]
    [SerializeField]
    private Toggle tieredGenerationEnabled = null;

    [SerializeField]
    private Slider tieredGenerationMemoryUpdate = null;

    [Header("Difficulty N-Gram")]
    [SerializeField]
    private Toggle difficultyNGramEnabled = null;

    [SerializeField]
    private Slider difficultyNGramMemoryUpdate = null;

    [SerializeField]
    private Slider leftColumns = null;

    [SerializeField]
    private Slider rightColumns = null;

    public Config Config { get; private set; }
    public bool StartCalled { get; private set; }
    public Button BackButton { get { return backButton; } }

    private void Awake()
    {
        StartCalled = false;

        Assert.IsNotNull(backButton);

        Assert.IsNotNull(custom);
        Assert.IsNotNull(superMarioBros);
        Assert.IsNotNull(superMarioBros2);
        Assert.IsNotNull(superMarioBros2Japan);
        Assert.IsNotNull(superMarioLand);

        Assert.IsNotNull(nLevel);
        Assert.IsNotNull(simplifiedNGramEnabled);
        Assert.IsNotNull(heiarchalNGramEanbled);

        Assert.IsNotNull(minLevelSize);
        Assert.IsNotNull(maxLevelSize);

        Assert.IsNotNull(tieredGenerationEnabled);
        Assert.IsNotNull(tieredGenerationMemoryUpdate);

        Assert.IsNotNull(difficultyNGramEnabled);
        Assert.IsNotNull(difficultyNGramMemoryUpdate);
        Assert.IsNotNull(leftColumns);
        Assert.IsNotNull(rightColumns);
    }

    private void Start()
    {
        Config = new Config
        {
            Game = Games.Custom,
            ProcedurallyGenerateLevels = procedurallyGenerateLevels.isOn,
            N = (int)nLevel.value,
            UsingSimplifiedNGram = simplifiedNGramEnabled.isOn,
            HeiarchalEnabled = heiarchalNGramEanbled.isOn,
            HeiarchalMemory = heiarchalMemory.value,
            MinLevelSize = (int)minLevelSize.value,
            MaxLevelSize = (int)maxLevelSize.value,
            UsingTieredGeneration = tieredGenerationEnabled.isOn,
            TieredGenerationMemoryUpdate = (int)tieredGenerationMemoryUpdate.value,
            DifficultyNGramEnabled = difficultyNGramEnabled.isOn,
            DifficultyNGramMemoryUpdate = difficultyNGramMemoryUpdate.value,
            DifficultyNGramLeftColumns = (int)leftColumns.value,
            DifficultyNGramRightColumns = (int)rightColumns.value
        };

        procedurallyGenerateLevels.onValueChanged.AddListener((bool val) => 
        {
            Config.ProcedurallyGenerateLevels = false;
        });

        custom.onClick.AddListener(() =>
        {
            Config.Game = Games.Custom;
        });

        superMarioBros.onClick.AddListener(() => 
        {
            Config.Game = Games.SuperMariosBros;
        });

        superMarioBros2.onClick.AddListener(() => 
        {
            Config.Game = Games.SuperMarioBros2;
        });

        superMarioBros2Japan.onClick.AddListener(() => 
        {
            Config.Game = Games.SuperMarioBros2Japan;
        });

        superMarioLand.onClick.AddListener(() => 
        {
            Config.Game = Games.SuperMarioLand;
        });

        nLevel.onValueChanged.AddListener((float val) =>
        {
            Config.N = (int)val;
        });

        simplifiedNGramEnabled.onValueChanged.AddListener((bool val) =>
        {
            Config.UsingSimplifiedNGram = val;
        });

        heiarchalNGramEanbled.onValueChanged.AddListener((bool val) =>
        {
            Config.HeiarchalEnabled = val;
        });

        heiarchalMemory.onValueChanged.AddListener((float val) =>
        {
            Config.HeiarchalMemory = val;
        });

        minLevelSize.onValueChanged.AddListener((float val) =>
        {
            Config.MinLevelSize = (int)val;
        });

        maxLevelSize.onValueChanged.AddListener((float val) =>
        {
            Config.MaxLevelSize = (int)val;
        });

        tieredGenerationEnabled.onValueChanged.AddListener((bool val) =>
        {
            Config.UsingTieredGeneration = val;
        });

        tieredGenerationMemoryUpdate.onValueChanged.AddListener((float val) => 
        {
            Config.TieredGenerationMemoryUpdate = val;
        });

        difficultyNGramEnabled.onValueChanged.AddListener((bool val) => 
        {
            Config.DifficultyNGramEnabled = val;
        });

        difficultyNGramMemoryUpdate.onValueChanged.AddListener((float val) => 
        {
            Config.DifficultyNGramMemoryUpdate = val;
        });

        leftColumns.onValueChanged.AddListener((float val) => 
        { 
            Config.DifficultyNGramLeftColumns = (int) val;
        });

        rightColumns.onValueChanged.AddListener((float val) => 
        { 
            Config.DifficultyNGramRightColumns = (int) val;
        });

        StartCalled = true;
    }
}