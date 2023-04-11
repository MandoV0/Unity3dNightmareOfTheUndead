using Assets.GameProject.Scripts;
using Assets.GameProject.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup showCanvas;
    [SerializeField] private Menu currentMenu;
    [SerializeField] private Menu[] menus;

    private PlayerStats playerStats;

    private void Start()
    {
        OnSpawnPlayer();
    }

    public void OnPlayerDeath()
    {
        OpenMenu(menus[2]);
    }

    private void OnSpawnPlayer()
    {
        playerStats = transform.root.GetComponent<PlayerStats>();
        OpenMenu(menus[0]);
    }

    private void OpenMenu(Menu menu)
    {
        foreach (Menu men in menus)
        {
            if (men == menu)
            {
                currentMenu = men;
                men.gameObject.SetActive(true);
            }
            else
            {
                men.gameObject.SetActive(false);
            }
        }
        showCanvas.alpha = 1;
    }

    public void OnToggleMenu()
    {
        if (playerStats.GetPlayerHealth().IsAlive())
        {
            if (currentMenu == menus[0])
            {
                OpenMenu(menus[3]);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                OpenMenu(menus[0]);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            OpenMenu(menus[2]);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        LoadingScreen.instance.LoadScene("MainMenu");
    }
}