using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Wave_FPS_Mobile.Scripts;

public class HUD : MonoBehaviour
{
    public static HUD instance;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI roundWaitingText;
    public Image flashScreen;

    [Header("Blood Overlay")]
    public Image BloodOverlay;
    public float pulseSpeed = 0.5f; // Speed of the pulse animation
    public float minValue = 0.5f; // Minimum value of the pulse
    public float maxValue = 1.0f; // Maximum value of the pulse
    private float currentValue = 0.0f; // Current value of the pulse
    private float bloodOverlayTimer = 0.0f;
    public float bloodOverlayTime = 5;

    private Coroutine currentFlash;

    [Header("Health")]
    public Image healthBar;
    public TextMeshProUGUI healthText;

    [Header("Perks")]
    [SerializeField] private Transform perkLayout;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (WaveSpawner.Instance)
        {
            RoundState roundState = WaveSpawner.Instance.GetRoundState();
            if (roundState == RoundState.Wait)
            {
                roundWaitingText.SetText("Waiting for next round...");
            }
            else if (roundState == RoundState.End) 
            {
                roundWaitingText.SetText("Game has ended");
            }
            else
            {
                roundWaitingText.SetText("");
            }
        }
        else
        {
            roundWaitingText.SetText("");
        }

        bloodOverlayTimer = Mathf.MoveTowards(bloodOverlayTimer, 0, Time.deltaTime);

        if (bloodOverlayTimer == 0)
        {
            currentValue = Mathf.MoveTowards(currentValue, 0, Time.deltaTime * 5);
        }
        else
        {
            // Use Mathf.PingPong function to create a looping value between minValue and maxValue
            currentValue = Mathf.PingPong(Time.time * pulseSpeed, maxValue - minValue) + minValue;
        }

        // Use the currentValue to animate your object or perform other operations
        Color color = BloodOverlay.color;
        color.a = currentValue;
        BloodOverlay.color = color;
    }

    public void SetHealth(int currentHealth, int maxHealth)
    {
        float perc = (float)currentHealth / (float)maxHealth;
        healthBar.fillAmount = perc;
        healthText.SetText(currentHealth.ToString());
    }

    public void SetRoundText(string text)
    {
        roundText.text = text;
    }

    public void SetPointsText(int points)
    {
        pointsText.text = points + "$";
    }

    public void FlashScreen(float secondsForOneFlash, float maxAlpha, Color newColor)
    {
        flashScreen.color = newColor;

        maxAlpha = Mathf.Clamp(maxAlpha, 0, 1);

        if (currentFlash != null)
        {
            StopCoroutine(currentFlash);
        }
        currentFlash = StartCoroutine(Flash(secondsForOneFlash, maxAlpha));
    }

    private IEnumerator Flash(float secondsForOneFlash, float maxAlpha)
    {
        float flashInDuration = secondsForOneFlash / 2;
        for (float i = 0; i <= flashInDuration; i += Time.deltaTime)
        {
            Color tmpColor = flashScreen.color;
            tmpColor.a = Mathf.Lerp(maxAlpha, 0, i / flashInDuration);
            flashScreen.color = tmpColor;
            yield return null;
        }

        flashScreen.color = new Color(0, 0, 0, 0);
    }

    /// <summary>
    /// Adds the given perk icon to the Player HUD
    /// </summary>
    /// <param name="perkIcon"></param>
    public void AddPerk(Sprite perkIcon)
    {
        GameObject perkImage = new GameObject("Perk Icon");
        perkImage.transform.parent = perkLayout;
        Image img = perkImage.AddComponent<Image>();
        perkImage.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
        img.sprite = perkIcon;
    }

    /// <summary>
    /// Removes all Perk icons
    /// </summary>
    public void RemovePerks()
    {
        foreach (Transform child in perkLayout)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void TakeDamage()
    {
        bloodOverlayTimer += bloodOverlayTime;
    }
}
