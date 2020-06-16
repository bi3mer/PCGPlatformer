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
    [Header("Loading Screen")]
    public GameObject LoadingScreen = null;

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
    public Button GotoMainMenuButton = null;

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
    public List<string> LevelIds;

    [HideInInspector]
    public bool Reset = true;

    private JsonArray pcgPlatfromerData = null;
    private JsonArray PCGPlatformerData
    {
        get
        {
            if (pcgPlatfromerData == null)
            {
                pcgPlatfromerData = LightJson.Serialization.JsonReader.Parse(PCGPlatformer.text);
            }

            return pcgPlatfromerData;
        }
    }

    private JsonArray superMarioBrosData = null;
    private JsonArray SuperMarioBrosData
    {
        get
        {
            if (superMarioBrosData == null)
            {
                superMarioBrosData = LightJson.Serialization.JsonReader.Parse(SuperMarioBros.text);
            }

            return superMarioBrosData;
        }
    }

    private JsonArray superMarioBros2Data = null;
    private JsonArray SuperMarioBros2Data
    {
        get
        {
            if (superMarioBros2Data == null)
            {
                superMarioBros2Data = LightJson.Serialization.JsonReader.Parse(SuperMarioBros2.text);
            }

            return superMarioBros2Data;
        }
    }

    private JsonArray superMarioBros2JapanData = null;
    private JsonArray SuperMarioBros2JapanData
    {
        get
        {
            if (superMarioBros2JapanData == null)
            {
                superMarioBros2JapanData = LightJson.Serialization.JsonReader.Parse(SuperMarioBros2Japan.text);
            }

            return superMarioBros2JapanData;
        }
    }

    private JsonArray superMarioLandData = null;
    private JsonArray SuperMarioLandData
    {
        get
        {
            if (superMarioLandData == null)
            {
                superMarioLandData = LightJson.Serialization.JsonReader.Parse(SuperMarioLand.text);
            }

            return superMarioLandData;
        }
    }

    public JsonArray GameFlow
    {
        get 
        {
            JsonArray gameFlow;
            switch (ConfigUI.Config.Game)
            {
                case Games.Custom:
                    gameFlow = PCGPlatformerData;
                    break;
                case Games.SuperMariosBros:
                    gameFlow = SuperMarioBrosData;
                    break;
                case Games.SuperMarioBros2:
                    gameFlow = SuperMarioBros2Data;
                    break;
                case Games.SuperMarioBros2Japan:
                    gameFlow = SuperMarioBros2JapanData;
                    break;
                case Games.SuperMarioLand:
                    gameFlow = SuperMarioLandData;
                    break;
                default:
                    gameFlow = PCGPlatformerData;
                    Debug.LogError($"\"{ConfigUI.Config.Game}\" unhandled game type");
                    break;
            }

            return gameFlow;
        }
    }

    private void Awake()
    {
        Assert.IsNotNull(LoadingScreen);

        Assert.IsNotNull(StartGameButton);
        Assert.IsNotNull(ConfigButton);

        Assert.IsNotNull(ConfigGameObject);
        Assert.IsNotNull(ConfigBackButton);
        Assert.IsNotNull(ConfigUI);

        Assert.IsNotNull(CameraFollow);
        Assert.IsNotNull(TryLevelAgainButton);

        Assert.IsNotNull(PlayLevelAgainButton);
        Assert.IsNotNull(GotoNextLevelButton);
        Assert.IsNotNull(GotoMainMenuButton);

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
