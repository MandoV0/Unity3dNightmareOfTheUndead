using UnityEngine.SceneManagement;
using UnityEngine;

public class DeathCanvas : MonoBehaviour
{
    public void RestartLevel()
    {
        LoadingScreen.instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        LoadingScreen.instance.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
