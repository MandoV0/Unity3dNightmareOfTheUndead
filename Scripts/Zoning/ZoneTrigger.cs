using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets.GameProject.Scripts;

namespace Zoning
{
    public class ZoneTrigger : MonoBehaviour
    {
        public Zone zone;
        public List<PlayerStats> playersInZone = new List<PlayerStats>();

        private void Awake()
        {
            if (zone == null)
            {
                zone = transform.parent.GetComponent<Zone>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if(playerStats != null)
            {
                if (!zone.hasBeenEnterd)
                {
                    zone.hasBeenEnterd = true;
                }
                
                playersInZone.Add(playerStats);
            }

            CheckIfPlayersAreInZone();
        }

        private void CheckIfPlayersAreInZone()
        {
            if(playersInZone.Count > 0)
            {
                zone.isActive = true;
            }
            else
            {
                zone.isActive = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playersInZone.Remove(playerStats);
            }

            CheckIfPlayersAreInZone();
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            BoxCollider box = transform.GetComponent<BoxCollider>();
            if (box)
            {
                Handles.Label(transform.TransformPoint(box.center), transform.parent.name);
            }
#endif
        }
    }
}
