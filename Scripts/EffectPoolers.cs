using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.GameProject.Scripts
{
    public class EffectPoolers : MonoBehaviour
    {
        public static EffectPoolers Instance;

        public List<EffectPooler> effectPools = new List<EffectPooler>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(Instance);
            }
        }

        public GameObject GetPooledObject(string effectTag)
        {
            foreach(EffectPooler pool in effectPools)
            {
                if (pool.effectTag.Equals(effectTag))
                {
                    return pool.GetPooledObject();
                }
            }
            return null;
        }
    }
}