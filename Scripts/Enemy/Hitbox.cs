using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private HitLayer hitLayer;

    public HitLayer GetHitLayer() 
    { 
        return hitLayer;
    }
}

public enum HitLayer
{
    Torso,
    Head
}