using Assets.GameProject.Scripts;
using Assets.GameProject.Scripts.Perks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMachine : InteractableBase
{
    [SerializeField] private PerkBehaviour perk;

    public override void Interact(PlayerStats player)
    {
        if(perk == null)
        {
            Debug.LogError($"Perk Machine : {name} has no perk attached!!!");
            return;
        }

        if (!player.HasPerk(perk))
        {
            if(player.RemovePoints(perk.cost))
            {
                player.AddPerk(perk);
            }
        }
        else
        {
            Debug.Log("Player already has this perk");
        }
    }

    public override string GetInteractionText(PlayerStats player)
    {
        if (perk == null)
        {
            return "";
        }

        if (player.HasPerk(perk))
        {
            return "You already have this perk";
        }

        return $"to buy {perk.perkName} [Cost: {perk.cost}]";
    }
}