using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cubinobi
{
    [CreateAssetMenu(fileName = "HitEffects", menuName = "Cubinobi/HitEffects", order = 0)]
    public class HitEffects : ScriptableObject
    {
        public bool isStunning;
        
        [ShowIf("isStunning")]
        public float stunDuration;
    }

}