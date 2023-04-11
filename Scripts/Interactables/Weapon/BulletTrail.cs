using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    public TrailRenderer trail;

    private Vector3 _targetPosition;

    public void Init(Vector3 hitPosition)
    {
        _targetPosition = hitPosition;
        StartCoroutine(StartTrail());
    }

    private IEnumerator StartTrail()
    {
        float time = 0;
        Vector3 spawnPos = transform.position;

        while (time < 1)
        {
            transform.position = Vector3.Lerp(spawnPos, _targetPosition, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        transform.position = _targetPosition;

        Destroy(gameObject, trail.time);
    }
}