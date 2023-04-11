using Assets.GameProject.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using UnityEngine;

public abstract class PowerUpBase : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 12;

    public void OnTriggerEnter(Collider other)
    {
        PlayerStats player = other.GetComponent<PlayerStats>();

        if (player != null)
        {
            OnPlayerTakePowerUp(player);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Activates the power up
    /// </summary>
    /// <param name="player"></param>
    public abstract void OnPlayerTakePowerUp(PlayerStats player);

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}