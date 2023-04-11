using System.Collections;
using UnityEngine;

namespace Assets.GameProject.Scripts.Perks
{
    [CreateAssetMenu(fileName = "New Perk Health", menuName = "Utilities/Perks/Health Perk")]
    public class PerkHealth : PerkBehaviour
    {
        [Header("Health Settings")]
        public int maxHealth = 200;

        private int previousMaxHealth;

        public override void OnAdd(PlayerStats player)
        {
            this.player = player;

            // Cache the current max health of the player
            previousMaxHealth = this.player.GetPlayerHealth().GetMaxHealth();

            this.player.GetPlayerHealth().SetMaxHealth(maxHealth);
            this.player.GetPlayerHealth().SetCurrentHealth(maxHealth);

            // Add Perk to the UI
            HUD.instance.AddPerk(perkImage);
        }

        public override void OnRemove()
        {
            // Set the health to the previous values
            this.player.GetPlayerHealth().SetMaxHealth(previousMaxHealth);
            this.player.GetPlayerHealth().SetCurrentHealth(previousMaxHealth);
        }
    }
}