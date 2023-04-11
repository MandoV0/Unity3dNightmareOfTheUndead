using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]

    [SerializeField] private int health = 100;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private bool isAlive;

    [Header("Take Damage")]

    [SerializeField] private AudioClip[] hurtSounds;
    [SerializeField] private Color hurtFlashColor;

    public UnityEvent OnDeath;

    private float _regenTimer;

    private void Start()
    {
        isAlive = true;
        SetCurrentHealth(health);
    }

    private void Update()
    {
        if (isAlive)
        {
            if (_regenTimer > 0f)
            {
                _regenTimer -= Time.deltaTime;
            }
            else
            {
                _regenTimer = 0f;
                if (health < maxHealth)
                {
                    SetCurrentHealth(health + 1);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) 
        { 
            return;
        }

        _regenTimer = 8;

        SetCurrentHealth(health -= damage);

        // HUD.instance.FlashScreen(0.8f, 0.5f, hurtFlashColor);
        HUD.instance.TakeDamage();

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isAlive = false;
        GetComponent<PlayerInput>().enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        OnDeath?.Invoke();
    }

    #region Get/Set
    public void SetCurrentHealth(int value)
    {
        health = value;
        HUD.instance.SetHealth(health, maxHealth);
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public void SetMaxHealth(int value)
    {
        maxHealth = value;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public int GetCurrentHealth()
    {
        return health;
    }

    public int GetMaxHealth() 
    {
        return maxHealth;
    }
    #endregion
}