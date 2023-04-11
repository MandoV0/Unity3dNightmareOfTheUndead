using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deactivates a gameObject after the time passes
/// </summary>
public class DeactivateAfterTime : MonoBehaviour
{
    [SerializeField] private float time = 25;

    private void OnEnable()
    {
        Invoke(nameof(Deactivate), time);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
