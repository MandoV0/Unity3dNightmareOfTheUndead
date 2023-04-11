using Assets.GameProject.Scripts;
using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteractor : MonoBehaviour
{
    // Player that gets passed to the interactable object
    [SerializeField] private PlayerStats playerStats;
    // The players camera transform from which we will trace
    [SerializeField] private Transform cameraTransform;
    // How far we can interact with interactables
    [SerializeField] private float interactionDistance = 1;
    // Radius of the interaction trace
    [SerializeField] private float radius = 1;

    public InteractableBase interactable;

    void Update()
    {
        //Interaction Trace.
        if (Physics.SphereCast(cameraTransform.position, radius, cameraTransform.forward, out var hitResult, interactionDistance))
        {
            //If we hit a collider.
            if (hitResult.collider != null)
            {
                //Try to get the interactable.
                interactable = hitResult.collider.GetComponent<InteractableBase>();
            }
            else
            {
                interactable = null;
            }
        }
        else
        {
            interactable = null;
        }
    }

    // Input action
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!CanInteract()) { return; }

            if (interactable != null) 
            {
                interactable.Interact(transform.root.GetComponent<PlayerStats>());
            }
        }
    }

    /// <summary>
    /// Checks if the player can interact (Maybe he is downed etc)
    /// </summary>
    /// <returns></returns>
    public bool CanInteract()
    {
        // TODO: Check if he can interact 
        return true;
    }

    public InteractableBase GetInteractable() 
    {
        return interactable;
    }
}
