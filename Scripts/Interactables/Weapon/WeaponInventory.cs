using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : InventoryBehaviour
{
    public WeaponBehaviour currentWeapon;

    public List<WeaponBehaviour> weapons;
    public int currentIndex = 0;

    private const int MaxWeapons = 2;

    public override WeaponBehaviour Equip(int index)
    {
        if (currentWeapon)
        {
            currentWeapon.gameObject.SetActive(false);
            currentWeapon = weapons[index];
            currentWeapon.gameObject.SetActive(true);
            currentIndex = index;
            return currentWeapon;
        }
        return null;
    }

    public override WeaponBehaviour GetEquipped()
    {
        return currentWeapon;
    }

    public WeaponBase GetCurrentWeapon()
    {
        return (WeaponBase)currentWeapon;
    }

    public override int GetEquippedIndex()
    {
        return currentIndex;
    }

    public override int GetLastIndex()
    {
        //Get last index with wrap around.
        int newIndex = currentIndex - 1;
        if (newIndex < 0)
            newIndex = weapons.Count - 1;

        //Return.
        return newIndex;
    }

    public override int GetNextIndex()
    {
        int newIndex = currentIndex + 1;
        if (newIndex > weapons.Count - 1)
            newIndex = 0;

        //Return.
        return newIndex;
    }

    public override void Init(int equippedAtStart = 0)
    {
        currentWeapon = GetComponentInChildren<WeaponBehaviour>();
        Equip(0);
    }

    public List<WeaponBehaviour> GetAllWeapons()
    {
        return weapons;
    }

    /// <summary>
    /// Add a new Weapon to the Inventory
    /// </summary>
    /// <param name="weapon"></param>
    public void AddWeapon(WeaponBehaviour weapon)
    {
        Character character = transform.root.GetComponent<Character>();

        // Destroy our current weapon if the amount of our weapons would
        // be more than the limit
        if (weapons.Count >= MaxWeapons)
        {
            currentWeapon.gameObject.SetActive(false);
            weapons.Remove(currentWeapon);
            Destroy(currentWeapon);
            currentWeapon = null;
        }
        else
        {
            currentWeapon.gameObject.SetActive(false);
        }

        // Instantiate Weapon with inventory as parent
        WeaponBehaviour TmpWeapon = Instantiate(weapon, transform);
        // Add weapon to list
        weapons.Add(TmpWeapon);
        TmpWeapon.gameObject.SetActive(false);
        currentWeapon = TmpWeapon;
        
        if (character.CanChangeWeapon())
            character.RunEquip(weapons.IndexOf(TmpWeapon));
    }
}