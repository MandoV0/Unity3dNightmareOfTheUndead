using Assets.GameProject.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public List<Menu> menus = new List<Menu>();

    public void Start()
    {
        OpenMenu(menus[0]);
    }

    public void OpenMenu(Menu menu)
    {
        foreach (Menu men in menus) 
        {
            if (men == menu) 
            {
                men.gameObject.SetActive(true);
            }
            else
            {
                men.gameObject.SetActive(false);
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}