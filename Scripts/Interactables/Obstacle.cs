using Assets.GameProject.Scripts;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UnityEngine.AI.NavMeshObstacle))]
public class Obstacle : InteractableBase
{
    [SerializeField] private int cost = 500;
    [SerializeField] private string obstacleName;

    public override void Interact(PlayerStats player)
    {
        if (player.RemovePoints(cost))
        {
            Debug.Log($"Player {player.name} bought door: {name}");
            OnUse?.Invoke(player);
            gameObject.SetActive(false);
        }
    }

    public override string GetInteractionText(PlayerStats player)
    {
        return $"to open {obstacleName} [Cost: {cost}]";
    }
}