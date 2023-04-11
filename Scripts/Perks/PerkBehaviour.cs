using System.Collections;
using UnityEngine;

namespace Assets.GameProject.Scripts.Perks
{
    public abstract class PerkBehaviour : ScriptableObject
    {

        [Header("Perk")]
        public string perkName;
        [Multiline] public string perkDescription;
        public Sprite perkImage;
        public int cost;
        protected PlayerStats player;

        /// <summary>   
        /// Cache PlayerStats and applys perk attributes
        /// </summary>
        /// <param name="player"></param>
        public abstract void OnAdd(PlayerStats player);

        /// <summary>
        /// Removes the perk from the player
        /// </summary>
        public abstract void OnRemove();
    }
}