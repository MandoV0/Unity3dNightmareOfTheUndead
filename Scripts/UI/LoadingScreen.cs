using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;
    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreenCanvas;
    [SerializeField] private Image loadingFillBar;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceenAsync(sceneName));
    }

    private IEnumerator LoadSceenAsync(string sceneName) 
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadingScreenCanvas.SetActive(true);

        while (!operation.isDone) 
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingFillBar.fillAmount = progressValue;
            yield return null;
        }

        loadingScreenCanvas.SetActive(false);
    }
}
