using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponEffectData", menuName = "Weapons/WeaponEffectData", order = 1)]
public class WeaponEffectData : ScriptableObject
{
    [Title(label: "Shooting Effects")]
    public BulletTrail bulletTrail;
}