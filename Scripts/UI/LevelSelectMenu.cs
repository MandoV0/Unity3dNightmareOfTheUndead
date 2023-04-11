using Assets.GameProject;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectMenu : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private LevelInfo levelInfo;

    [Header("UI")]
    [SerializeField] private LevelSelectButton levelSelectButtonPrefab;
    [SerializeField] private Transform layoutTransform;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI levelDescription;
    [SerializeField] private Image levelImage;

    private void Awake()
    {
        foreach (Level level in levelInfo.levels)
        {
            LevelSelectButton levelSelectButton = Instantiate(levelSelectButtonPrefab, layoutTransform);
            levelSelectButton.Init(level, this);
            levelSelectButton.GetComponent<Button>().onClick.AddListener( delegate { LoadLevel(level); } );
            levelSelectButton.GetComponentInChildren<TextMeshProUGUI>().text = level.name;
        }

        // Load the Data of the first level
        LoadLevelData(levelInfo.levels[0]);
    }

    public void LoadLevelData(Level level)
    {
        Debug.Log("Loading level data: " + level.name);
        levelName.text = level.name;
        levelDescription.text = level.description;
        levelImage.sprite = level.levelSprite;
    }

    public void LoadLevel(Level level)
    {
        Debug.Log("Loading level async: " + level.name);
        LoadingScreen.instance.LoadScene(level.sceneName);
    }
}