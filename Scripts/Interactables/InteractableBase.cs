using Assets.GameProject.Scripts;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableBase : MonoBehaviour
{
    [Title(label: "Interactable")]
    [SerializeField] private string interactionText;
    [SerializeField] protected UnityEvent<PlayerStats> OnUse;

    public abstract void Interact(PlayerStats player);

    public virtual string GetInteractionText(PlayerStats player)
    {
        return interactionText;
    }
}
