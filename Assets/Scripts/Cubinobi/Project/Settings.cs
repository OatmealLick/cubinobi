using Sirenix.OdinInspector;
using UnityEngine;

namespace Cubinobi.Project
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Cubinobi/Settings", order = 0)]
    public class Settings : ScriptableObject
    {
        [Title("Movement Parameters")]
        public float movementSpeed = 6.0f;

        [Range(0f, 1f), InfoBox("Some controllers have worn out sticks or are very fragile and pick up on slightest stick movements. This is a deadzone threshold that will be used to filter out the analog stick movements that are below that value.")]
        public float deadzoneInputThreshold = 0.2f;
        
        [Title("Jump Parameters")]
        [Range(1, 10), InfoBox("How high the character can reach in the peak of the jump")]
        public float jumpHeight = 5.0f;
        
        [Range(0.1f, 1.0f), InfoBox("Time it takes to reach the peak height of the jump.")]
        public float jumpTimeToPeak = 0.4f;
        
        [Range(1.0f, 2.0f),
         InfoBox("When character is falling down this multiplies the gravity to achieve Hollow Knight / Mario effect.")]
        public float jumpFallingGravityMultiplier = 1.4f;

        [InfoBox("Variable jump gravity multiplier. When player releases the jump key before reaching peak, this multiplies the gravity to achieve variable height jump.")]
        public float jumpVariableHeightGravityMultiplier = 1.8f;

        [Title("Attack Parameters")]
        [InfoBox("Modifies the size of the collider that is being used to check whether an enemy was hit by an attack.")]
        public Vector2 attackColliderSize = new(1.8f, 1.2f);

        [InfoBox("Time in seconds for melee attack flash. Purely visual setting, does not effect mechanics. (attack happens instantaneously)")]
        public float attackMeleeGizmoFlashDuration = 0.3f;
    }
}