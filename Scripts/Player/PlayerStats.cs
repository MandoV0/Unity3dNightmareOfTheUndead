using Assets.GameProject.Scripts.Perks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameProject.Scripts
{
    public class PlayerStats : MonoBehaviour
    {
        public int playerId;
        [SerializeField] private int points = 500;

        [SerializeField] private PlayerHealth playerHealth;

        [SerializeField] private List<PerkBehaviour> perks = new List<PerkBehaviour>();

        private void Awake()
        {
            // Loads and setups the Player Preferences like Sensitivity, Graphics ets
            PlayerPrefsWriter.LoadPlayerPrefs();
            PlayerPrefsWriter.GetMasterVolume();
            PlayerPrefsWriter.GetGraphics();

        }

        public void Start()
        {
            playerHealth = GetComponent<PlayerHealth>();
            HUD.instance.SetPointsText(points);
        }

        /// <summary>
        /// Returns true if we have enough to buy and removes the amount from the points
        /// </summary>
        /// <param name="amount"></param>
        /// <returns> False if we dont have enough ammo </returns>
        public bool RemovePoints(int amount)
        {
            if(points - amount < 0)
            {
                return false;
            }

            points -= amount;
            HUD.instance.SetPointsText(points);

            return true;
        }

        public int GetPoints()
        {
            return points;
        }

        public void AddPoints(int amount) 
        {
            points += amount;
            HUD.instance.SetPointsText(points);
        }

        public PlayerHealth GetPlayerHealth()
        {
            return playerHealth;
        }

        /// <summary>
        /// Returns true if the player has the perk
        /// </summary>
        /// <param name="perk"></param>
        /// <returns></returns>
        public bool HasPerk(PerkBehaviour perk)
        {
            return perks.Contains(perk);
        }

        /// <summary>
        /// Adds and activates the perk and returns true if the perk has been added
        /// </summary>
        /// <param name="perk"> The perk to add</param>
        /// <returns> if the perk has been added</returns>
        public bool AddPerk(PerkBehaviour perk) 
        {
            if (HasPerk(perk))
            {
                Debug.LogWarning($"Player already has perk: {perk.perkName}");
                return false;
            }

            perks.Add(perk);
            perk.OnAdd(this);
            return true;
        }
    }
}