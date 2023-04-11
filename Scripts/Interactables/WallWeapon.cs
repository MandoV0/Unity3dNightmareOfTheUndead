using InfimaGames.LowPolyShooterPack;
using System.Collections;
using UnityEngine;

namespace Assets.GameProject.Scripts
{
    public class WallWeapon : InteractableBase
    {
        [SerializeField] private int cost;
        [SerializeField] private int ammoCost;
        [SerializeField] private WeaponData buyWeapon;

        public override void Interact(PlayerStats player)
        {
            if (buyWeapon == null)
            {
                Debug.LogError(name + " has no buyable weapon attached");
                return;
            }

            // Cache the Inventory
            WeaponInventory weaponInventory = player.transform.root.GetComponentInChildren<WeaponInventory>();
            
            // If we dont have this weapon equiped check if we have enough money and add it to our inventory
            if (weaponInventory.GetCurrentWeapon().GetWeaponData() != buyWeapon)
            {
                if (player.RemovePoints(cost))
                {
                    Debug.Log($"Player {player.name} has bought weapon at wall weapon {name}");
                    weaponInventory.AddWeapon(buyWeapon.weaponPrefab);
                }
            }
            else
            {
                WeaponBase currentWeapon = weaponInventory.GetCurrentWeapon();
                // Buy ammo if the ammo isnt full and we have enough money
                if(!currentWeapon.HasFullAmmo())
                {
                    if(player.RemovePoints(ammoCost))
                    {
                        currentWeapon.RefillAmmo();
                    }
                }
            }
        }

        public override string GetInteractionText(PlayerStats player)
        {
            if (buyWeapon == null)
            {
                Debug.LogError(name + " has no buyable weapon attached");
                return "";
            }

            WeaponInventory weaponInventory = player.transform.root.GetComponentInChildren<WeaponInventory>();

            if (weaponInventory.GetCurrentWeapon().GetWeaponData() == buyWeapon)
            {
                return $"to buy ammo for {buyWeapon.weaponName} [Cost: {ammoCost}]";
            }
            else
            {
                return $"for {buyWeapon.weaponName} [Cost: {cost}]";
            }
        }
    }
}