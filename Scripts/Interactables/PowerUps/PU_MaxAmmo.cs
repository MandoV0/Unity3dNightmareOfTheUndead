using Assets.GameProject.Scripts;
using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_MaxAmmo : PowerUpBase
{
    public override void OnPlayerTakePowerUp(PlayerStats player)
    {
        player.GetComponent<Character>().SetGrenadeCount(3);
        player.GetComponentInChildren<WeaponInventory>().GetAllWeapons().ForEach(weapon => { weapon.RefillAmmo(); } );
    }
}