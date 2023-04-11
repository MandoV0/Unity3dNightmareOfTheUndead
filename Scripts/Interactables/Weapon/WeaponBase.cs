using Assets.GameProject.Scripts;
using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : WeaponBehaviour
{
    [Title(label: "Weapon Data")]
    [SerializeField]
    private WeaponData weaponData;
    private Transform playerCamera;

    [Title(label: "Weapon Animation")]
    [Tooltip("Transform that represents the weapon's ejection port, meaning the part of the weapon that casings shoot from.")]
    [SerializeField]
    private Transform socketEjection;

    [Title(label: "Weapon Runtime Data")]
    public int currentMagazineAmmo;
    public int currentSpareAmmo;
    
    // Weapon animator
    private Animator animator;
    private IGameModeService gameModeService;
    private CharacterBehaviour characterBehaviour;
    private MuzzleBehaviour muzzleBehaviour;
    private PlayerStats playerStats;

    public void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        //Cache the game mode service. We only need this right here, but we'll cache it in case we ever need it again.
        gameModeService = ServiceLocator.Current.Get<IGameModeService>();
        //Cache the player character.
        characterBehaviour = gameModeService.GetPlayerCharacter();
        //Cache the world camera. We use this in line traces.
        playerCamera = characterBehaviour.GetCameraWorld().transform;
        //Cache the player stats.
        playerStats = transform.root.GetComponent<PlayerStats>();
    }

    public void Start()
    {
        base.Start();
        currentMagazineAmmo = weaponData.magazineAmmo;
        currentSpareAmmo = weaponData.startSpareAmmo;

        muzzleBehaviour = GetComponent<WeaponAttachmentManager>().GetEquippedMuzzle();
    }

    /// <summary>
    /// Reload.
    /// </summary>
    public override void Reload()
    {
        //Set Reloading Bool. This helps cycled reloads know when they need to stop cycling.
        const string boolName = "Reloading";
        animator.SetBool(boolName, true);

        //Try Play Reload Sound.
        ServiceLocator.Current.Get<IAudioManagerService>().PlayOneShot(HasAmmunition() ? weaponData.audioClipReload : weaponData.audioClipReloadEmpty, new InfimaGames.LowPolyShooterPack.AudioSettings(1.0f, 0.0f, false));

        //Play Reload Animation.
        animator.Play(weaponData.cycledReload ? "Reload Open" : (currentMagazineAmmo > 0 ? "Reload" : "Reload Empty"), 0, 0.0f);
    }

    public override void Fire(float spreadMultiplier = 1.0f)
    {
        //Play the firing animation.
        const string stateName = "Fire";
        animator.Play(stateName, 0, 0.0f);

        //Reduce ammunition! We just shot, so we need to get rid of one!
        currentMagazineAmmo = Mathf.Clamp(currentMagazineAmmo - 1, 0, weaponData.magazineAmmo);

        //Set the slide back if we just ran out of ammunition.
        if (currentMagazineAmmo == 0)
            SetSlideBack(1);

        //Play all muzzle effects.
        muzzleBehaviour.Effect();

        // How many shots come out at once
        for (var i = 0; i < weaponData.shotCount; i++)
        {
            //Determine a random spread value using all of our multipliers.
            // Vector3 spreadValue = Random.insideUnitSphere * (weaponData.spread * spreadMultiplier);

            //Remove the forward spread component, since locally this would go inside the object we're shooting!
            // spreadValue.z = 0;

            //Convert to world space.
            // spreadValue = playerCamera.TransformDirection(spreadValue);

            // How many shots come out at once
            Raycast();
        }
    }

    private void Raycast()
    {
        var cameraTransform = playerCamera.transform;
        var pos = cameraTransform.position;
        var dir = cameraTransform.forward;
        dir += cameraTransform.TransformDirection(CalcSpread());
        float range = 500;
        int targets = 0;

        // The maximum amoung of targets we can penetrate
        const int maxPenTargets = 5;

        while (targets < maxPenTargets && Physics.Raycast(pos, dir, out var hit, range, ~LayerMask.GetMask("Character")))
        {
            targets++;
            range -= hit.distance;
            pos = hit.point;

            EnemyBase enemy = hit.collider.transform.root.GetComponent<EnemyBase>();
            BulletTrail bulletTrailPrefab = weaponData.weaponEffectData.bulletTrail;

            if (bulletTrailPrefab != null) 
            {
                BulletTrail bulletTrail = Instantiate(bulletTrailPrefab, muzzleBehaviour.transform.position, Quaternion.identity);
                bulletTrail.Init(hit.point);
            }

            if (enemy != null)
            {
                float damage = GetDamage(hit.distance);
                damage *= GetDamageMultiplier(hit) / targets;
                HitLayer hitLayer = hit.collider.GetComponent<Hitbox>().GetHitLayer();
                enemy.TakeDamage(Mathf.CeilToInt(damage), playerStats, dir, hitLayer, hit);
                
                if (hit.transform.CompareTag("Blood"))
                {
                    GameObject bloodImpact = EffectPoolers.Instance.GetPooledObject("IMP_Blood");

                    if (bloodImpact != null)
                    {
                        bloodImpact.transform.SetLocalPositionAndRotation(hit.transform.position, Quaternion.LookRotation(hit.normal));
                        bloodImpact.SetActive(true);
                    }
                }
            }
            else
            {
                break;
            }
        }
    }

    /// <summary> Calculates spread </summary>
    private Vector2 CalcSpread()
    {
        float aimingProgress = 0;
        Vector2 hipSpread = weaponData.spread * 0.6f * RandomPointInCirclePolar();
        Vector2 adsSpread = hipSpread * weaponData.aimingSpreadMultiplier;
        if (characterBehaviour.IsAiming())
        {
            aimingProgress = 1;
        }

        return Vector2.Lerp(hipSpread, adsSpread, aimingProgress);
    }

    /// <summary>
    /// Generates a Random number using the the Polar Coordinates Model. The Points are more densely packed in the center of the circle
    /// </summary>
    /// <returns> Random Vector2 between 0 and 1 </returns>
    private Vector2 RandomPointInCirclePolar()
    {
        float theta = Random.Range(0f, 1f) * 2 * Mathf.PI;
        float r = Random.Range(0f, 1f);
        return new Vector2(r * Mathf.Cos(theta), r * Mathf.Sin(theta));
    }

    /// <summary>
    /// Calculates the damage multiplier
    /// </summary>
    /// <param name="hit"></param>
    /// <returns> damage multiplier </returns>
    public float GetDamageMultiplier(RaycastHit hit)
    {
        Hitbox hitbox = hit.collider.GetComponent<Hitbox>();
        float multiplier = 1;

        if (hitbox)
        {
            switch (hitbox.GetHitLayer())
            {
                case HitLayer.Torso:
                    multiplier = 1;
                    break;
                case HitLayer.Head:
                    multiplier = 2;
                    break;
            }
        }

        return multiplier;
    }

    public int GetDamage(float distance = 0)
    {
        return Mathf.CeilToInt(weaponData.damageCurve.Evaluate(distance));
    }

    /// <summary>
    /// SetSlideBack.
    /// </summary>
    public override void SetSlideBack(int back)
    {
        //Set the slide back bool.
        const string boolName = "Slide Back";
        animator.SetBool(boolName, back != 0);
    }

    public override Sprite GetSpriteBody()
    {
        return weaponData.spriteBody;
    }

    public override float GetMultiplierMovementSpeed()
    {
        return weaponData.multiplierMovementSpeed;
    }

    public override AudioClip GetAudioClipHolster()
    {
        return weaponData.audioClipHolster;
    }

    public override AudioClip GetAudioClipUnholster()
    {
        return weaponData.audioClipUnholster;
    }

    public override AudioClip GetAudioClipReload()
    {
        return weaponData.audioClipReload;
    }

    public override AudioClip GetAudioClipReloadEmpty()
    {
        return weaponData.audioClipReloadEmpty;
    }

    public override AudioClip GetAudioClipReloadOpen()
    {
        return weaponData.audioClipReloadOpen;
    }

    public override AudioClip GetAudioClipReloadInsert()
    {
        return weaponData.audioClipReloadInsert;
    }

    public override AudioClip GetAudioClipReloadClose()
    {
        return weaponData.audioClipReloadClose;
    }

    public override AudioClip GetAudioClipFireEmpty()
    {
        return weaponData.audioClipFireEmpty;
    }

    public override AudioClip GetAudioClipBoltAction()
    {
        return weaponData.audioClipBoltAction;
    }

    public override AudioClip GetAudioClipFire()
    {
        return muzzleBehaviour.GetAudioClipFire();
    }

    public override int GetAmmunitionCurrent()
    {
        return currentMagazineAmmo;
    }

    public override int GetAmmunitionTotal()
    {
        return weaponData.magazineAmmo;
    }

    public override bool HasCycledReload()
    {
        return weaponData.cycledReload;
    }

    public override Animator GetAnimator()
    {
        return animator;
    }

    public override bool CanReloadAimed()
    {
        return weaponData.canReloadAimed;
    }

    public override bool IsAutomatic()
    {
        return weaponData.fireMode == FireMode.Automatic;
    }

    public override bool HasAmmunition()
    {
        return currentMagazineAmmo > 0;
    }

    public override bool IsFull()
    {
        return currentMagazineAmmo == weaponData.magazineAmmo;
    }

    public override bool IsBoltAction()
    {
        return weaponData.boltAction;
    }

    public override bool GetAutomaticallyReloadOnEmpty()
    {
        return false;
    }

    public override float GetAutomaticallyReloadOnEmptyDelay()
    {
        return 0;
    }

    public override bool CanReloadWhenFull()
    {
        return false;
    }

    public override float GetRateOfFire()
    {
        return weaponData.roundPerMinute;
    }

    public override float GetFieldOfViewMultiplierAim()
    {
        return 1;
    }

    public override float GetFieldOfViewMultiplierAimWeapon()
    {
        return 1;
    }

    public override RuntimeAnimatorController GetAnimatorController()
    {
        return weaponData.controller;
    }

    public override WeaponAttachmentManagerBehaviour GetAttachmentManager()
    {
        return GetComponent<WeaponAttachmentManagerBehaviour>();
    }

    public override void FillAmmunition(int amount)
    {       
        if (amount != 0)
        {
            if (currentSpareAmmo > 0 && currentMagazineAmmo < weaponData.magazineAmmo)
            {
                currentMagazineAmmo += 1;
                currentSpareAmmo--;

                if (currentSpareAmmo + 1 == currentMagazineAmmo || currentSpareAmmo - 1 == 0)
                {
                    //Update the character animator.
                    characterBehaviour.GetCharacterAnimator().SetBool("Reloading", false);
                    //Update the weapon animator.
                    animator.SetBool("Reloading", false);
                }
            }
            else
            {
                //Update the character animator.
                characterBehaviour.GetCharacterAnimator().SetBool("Reloading", false);
                //Update the weapon animator.
                animator.SetBool("Reloading", false);
            }
            return;
        }

        currentSpareAmmo += currentMagazineAmmo;
        currentSpareAmmo -= weaponData.magazineAmmo;
        currentMagazineAmmo = weaponData.magazineAmmo;

        if (currentSpareAmmo < 0)
        {
            currentMagazineAmmo += currentSpareAmmo;
            currentSpareAmmo = 0;
        };
    }

    public override void EjectCasing()
    {
        //Spawn casing prefab at spawn point.
        if (weaponData.prefabCasing != null && socketEjection != null)
            Instantiate(weaponData.prefabCasing, socketEjection.position, socketEjection.rotation);
    }

    /// <summary>
    /// Chekcks if our reserve ammo is full
    /// </summary>
    /// <returns> True if reserve ammo is full </returns>
    public bool HasFullAmmo()
    {
        if (currentSpareAmmo == weaponData.maxSpareAmmo)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Returns the WeaponData
    /// </summary>
    /// <returns>WeaponData</returns>
    public WeaponData GetWeaponData()
    {
        return weaponData;
    }

    public override int GetSpareAmmunition()
    {
        return currentSpareAmmo;
    }

    private void OnDestroy()
    {
        characterBehaviour.GetCharacterAnimator().SetBool("Reloading", false);
        //Update the weapon animator.
        animator.SetBool("Reloading", false);
        characterBehaviour.AnimationEndedReload();
    }

    private void OnDisable()
    {
        characterBehaviour.GetCharacterAnimator().SetBool("Reloading", false);
        //Update the weapon animator.
        animator.SetBool("Reloading", false);
        characterBehaviour.AnimationEndedReload();
    }

    public override void RefillAmmo()
    {
        currentSpareAmmo = weaponData.maxSpareAmmo;
    }
}