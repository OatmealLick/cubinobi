using Sirenix.OdinInspector;
using UnityEngine;

namespace Cubinobi
{
    [CreateAssetMenu(fileName = "EnemiesSettings", menuName = "Cubinobi/EnemiesSettings", order = 0)]
    public class EnemiesSettings : ScriptableObject
    {
        [Title("Charging Enemy")]
        public float ceMovementSpeed = 4f;
        public float ceChargingSpeed = 9f;
        public float ceChargeLoadingTime = 0.8f;
        public float ceChargingTime = 2f;
        public float ceTimeToPatrolBeforeSwitchingDirections = 5f;
        public float ceReactThePlayerDistance = 5f;
        public float ceRecoveryAfterChargingTime = 0.6f;
        public int ceHealth = 10;
        public int ceDamage = 0;
        public HitEffects ceEffects;
    }
}