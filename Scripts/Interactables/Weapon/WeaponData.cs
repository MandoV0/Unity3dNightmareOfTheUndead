using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public WeaponBase weaponPrefab;

    [Title(label: "Settings")]

    [Tooltip("Weapon Name. Will be used for interacting with buy stations")]
    public string weaponName;
    public float multiplierMovementSpeed = 1f;

    [Title(label: "Firing")]
    public FireMode fireMode;

    public bool boltAction;

    [Tooltip("How many Projectiles get fired in one go")]
    public int shotCount = 1;

    [Tooltip("How far the weapon can fire from the center of the screen.")]
    public float spread = 0.25f;

    [Tooltip("How much of the original spread we have when aiming")]
    [Range(0f, 1f)]
    public float aimingSpreadMultiplier = 0;

    [Tooltip("How fast the weapon can fire")]
    public int roundPerMinute = 600;

    [Tooltip("Determines if this weapon reloads in cycles, meaning that it inserts one bullet at a time, or not.")]
    public bool cycledReload;

    [Title(label: "Ammunition")]
    public int magazineAmmo = 30;
    public int maxSpareAmmo = 210;
    public int startSpareAmmo = 120;

    [Title(label: "Damage")]
    public MinMaxCurve damageCurve;

    [Title(label: "Animation")]

    [Tooltip("The AnimatorController a player character needs to use while wielding this weapon. PCH")]
    public RuntimeAnimatorController controller;

    [Tooltip("Settings this to false will stop the weapon from being reloaded while the character is aiming it.")]
    public bool canReloadAimed = true;

    [Title(label: "Resources/Prefabs")]

    [Tooltip("Casing Prefab.")]
    public GameObject prefabCasing;

    [Tooltip("Weapon Body Texture.")]
    public Sprite spriteBody;

    [Title(label: "Audio Clips Holster")]

    public AudioClip audioClipHolster;

    [Tooltip("Unholster Audio Clip.")]
    public AudioClip audioClipUnholster;

    [Title(label: "Audio Clips Reloads")]

    [Tooltip("Reload Audio Clip.")]
    public AudioClip audioClipReload;

    [Tooltip("Reload Empty Audio Clip.")]
    public AudioClip audioClipReloadEmpty;

    [Title(label: "Audio Clips Reloads Cycled")]

    [Tooltip("Reload Open Audio Clip.")]
    public AudioClip audioClipReloadOpen;

    [Tooltip("Reload Insert Audio Clip.")]
    public AudioClip audioClipReloadInsert;

    [Tooltip("Reload Close Audio Clip.")]
    public AudioClip audioClipReloadClose;

    [Title(label: "Audio Clips Other")]

    [Tooltip("AudioClip played when this weapon is fired without any ammunition.")]
    public AudioClip audioClipFireEmpty;

    [Tooltip("")]
    public AudioClip audioClipBoltAction;

    [Title(label: "Effect Data")]
    public WeaponEffectData weaponEffectData;
}

public enum FireMode
{
    Automatic,
    Semiautomatic,
    Salve
}