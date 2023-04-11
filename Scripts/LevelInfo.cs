using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GameProject
{
    [CreateAssetMenu(fileName = "Levels", menuName = "Level/LevelInfo", order = 1)]
    public class LevelInfo : ScriptableObject
    {
        public List<Level> levels = new List<Level>();
    }

    [System.Serializable]
    public struct Level
    {
        public string name;
        [TextArea]
        public string description;
        public Sprite levelSprite;
        public string sceneName;
    }
}