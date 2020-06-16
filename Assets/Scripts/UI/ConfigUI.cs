using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

public class ConfigUI : MonoBehaviour
{
    [Header("Game Buttons")]
    [SerializeField]
    private Button Custom = null;

    [SerializeField]
    private Button SuperMarioBros = null;

    [SerializeField]
    private Button SuperMarioBros2 = null;

    [SerializeField]
    private Button SuperMarioBros2Japan = null;

    [SerializeField]
    private Button SuperMarioLand = null;

    [Header("Configuration Variables")]
    [SerializeField]
    private Slider NLevel = null;

    [SerializeField]
    private Toggle SimplifiedNGramEnabled = null;

    [Header("Level Sizes")]
    [SerializeField]
    private Slider MinLevelSize = null;

    [SerializeField]
    private Slider MaxLevelSize = null;

    [Header("Tiered Generation")]
    [SerializeField]
    private Toggle TieredGenerationEnabled = null;

    [SerializeField]
    private Slider TieredGenerationMemoryUpdate = null;

    [Header("Difficulty N-Gram")]
    [SerializeField]
    private Toggle DifficultyNGramEnabled = null;

    [SerializeField]
    private Slider DifficultyNGramMemoryUpdate = null;

    [SerializeField]
    private Slider LeftColumns = null;

    [SerializeField]
    private Slider RightColumns = null;

    public Config Config { get; private set; }

    private void Awake()
    {
        Assert.IsNotNull(Custom);
        Assert.IsNotNull(SuperMarioBros);
        Assert.IsNotNull(SuperMarioBros2);
        Assert.IsNotNull(SuperMarioBros2Japan);
        Assert.IsNotNull(SuperMarioLand);

        Assert.IsNotNull(NLevel);
        Assert.IsNotNull(SimplifiedNGramEnabled);

        Assert.IsNotNull(MinLevelSize);
        Assert.IsNotNull(MaxLevelSize);

        Assert.IsNotNull(TieredGenerationEnabled);
        Assert.IsNotNull(TieredGenerationMemoryUpdate);

        Assert.IsNotNull(DifficultyNGramEnabled);
        Assert.IsNotNull(DifficultyNGramMemoryUpdate);
        Assert.IsNotNull(LeftColumns);
        Assert.IsNotNull(RightColumns);
    }

    private void Start()
    {
        Config = new Config
        {
            Game = Games.Custom,
            N = (int)NLevel.value,
            UsingSimplifiedNGram = SimplifiedNGramEnabled.isOn,
            MinLevelSize = (int)MinLevelSize.value,
            MaxLevelSize = (int)MaxLevelSize.value,
            UsingTieredGeneration = TieredGenerationEnabled.isOn,
            TieredGenerationMemoryUpdate = (int)TieredGenerationMemoryUpdate.value,
            DifficultyNGramEnabled = DifficultyNGramEnabled.isOn,
            DifficultyNGramMemoryUpdate = DifficultyNGramMemoryUpdate.value,
            DifficultyNGramLeftColumns = (int)LeftColumns.value,
            DifficultyNGramRightColumns = (int)RightColumns.value
        };

        Custom.onClick.AddListener(() =>
        {
            Config.Game = Games.Custom;
        });

        SuperMarioBros.onClick.AddListener(() => 
        {
            Config.Game = Games.SuperMariosBros;
        });

        SuperMarioBros2.onClick.AddListener(() => 
        {
            Config.Game = Games.SuperMarioBros2;
        });

        SuperMarioBros2Japan.onClick.AddListener(() => 
        {
            Config.Game = Games.SuperMarioBros2Japan;
        });

        SuperMarioLand.onClick.AddListener(() => 
        {
            Config.Game = Games.SuperMarioLand;
        });

        NLevel.onValueChanged.AddListener((float val) =>
        {
            Config.N = (int)val;
        });

        MinLevelSize.onValueChanged.AddListener((float val) =>
        {
            Config.MinLevelSize = (int)val;
        });

        MaxLevelSize.onValueChanged.AddListener((float val) =>
        {
            Config.MaxLevelSize = (int)val;
        });

        TieredGenerationEnabled.onValueChanged.AddListener((bool val) =>
        {
            Config.UsingTieredGeneration = val;
        });

        TieredGenerationMemoryUpdate.onValueChanged.AddListener((float val) => 
        {
            Config.TieredGenerationMemoryUpdate = val;
        });

        DifficultyNGramEnabled.onValueChanged.AddListener((bool val) => 
        {
            Config.DifficultyNGramEnabled = val;
        });

        DifficultyNGramMemoryUpdate.onValueChanged.AddListener((float val) => 
        {
            Config.DifficultyNGramMemoryUpdate = val;
        });

        LeftColumns.onValueChanged.AddListener((float val) => 
        { 
            Config.DifficultyNGramLeftColumns = (int) val;
        });

        RightColumns.onValueChanged.AddListener((float val) => 
        { 
            Config.DifficultyNGramRightColumns = (int) val;
        });
    }
}