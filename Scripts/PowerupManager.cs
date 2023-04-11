using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager instance;
    [Header("Power Up Settings")]
    // PowerUps Prefabs start with the prefix "PU_"
    [SerializeField] private PowerUpBase[] powerUps;
    [SerializeField] private int dropPercantage = 1;
    [SerializeField] private int maxPowerUpsPerRound = 4;
    [Space(5)]
    private int powerUpsThisRound = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    /// <summary>
    /// Drops random power up with given chance
    /// Called from zombie when he dies
    /// </summary>
    /// <param name="position"> Where to spawn the power up </param>
    public void DropPowerUp(Vector3 position)
    {
        // if we have power ups in the array
        if (powerUps.Length > 0)
        {
            // if we droped less power ups then the allowed amount
            if (powerUpsThisRound < maxPowerUpsPerRound)
            {
                float rand = Random.Range(0, 100);

                if (rand <= dropPercantage)
                {
                    powerUpsThisRound++;
                    // Spawn random power up
                    position += new Vector3(0, 1.2f, 0);
                    Instantiate(powerUps[Random.Range(0, powerUps.Length)], position, Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.LogError("The PowerupManager has no PowerUps assigned");
        }
    }

    /// <summary>
    /// Event which gets called when subscribed tp the GameModes NewRound Event.
    /// This Function resets the necassary variables to spawn PowerUps in the new round.
    /// </summary>
    public void OnNewRound()
    {
        powerUpsThisRound = 0;
    }
}