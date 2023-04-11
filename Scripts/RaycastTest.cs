using Assets.GameProject.Scripts;
using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour
{
    public float maxRandDegree = 0.2f;
    public int iterations = 100;
    float range = 3;

    [ContextMenu("Do Tests")]
    public void DoTests()
    {
        for (int i = 0; i < iterations; i++) 
        {
            Cast();
        }
    }

    [ContextMenu("Cast Ray in Dir with Rand")]
    public void Cast()
    {
        var dirTransfrom = transform;

        var pos = dirTransfrom.position;
        var dir = dirTransfrom.forward;

        dir += dirTransfrom.TransformDirection(maxRandDegree * Random.insideUnitSphere);
        

        if (Physics.Raycast(pos, dir, out var hit, range))
        {
            Debug.DrawLine(pos, hit.point, Color.green, 8);
        }
        else
        {
            Debug.DrawRay(pos, dir * range, Color.red, 8);
        }
        
    }

    private void OnDrawGizmos()
    {
        // Forward Ray
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}
