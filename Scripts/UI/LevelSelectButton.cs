using Assets.GameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectButton : MonoBehaviour, IPointerEnterHandler
{
    private LevelSelectMenu levelSelectMenu;
    [SerializeField] private Level level;

    public void Init(Level level, LevelSelectMenu levelSelectMenu)
    {
        this.level = level;
        this.levelSelectMenu = levelSelectMenu;
    }

    public Level GetLevel()
    {
        return level;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        levelSelectMenu.LoadLevelData(level);
    }
}