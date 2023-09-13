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

        [Range(0.0f, 1.0f), InfoBox("Percentage of time to peak. If player released jump button before this, then it's a small jump, if not it's a big jump. If you set it to 0.5 it means that if player releases a button in first half of time to peak it will be a small jump.")]
        public float timeToReleaseForSmallJumpAsPercentageOfTimeToPeak = 0.33f;

        [InfoBox("Small jump gravity multiplier. When player releases the jump key before reaching peak, this multiplies the gravity to achieve variable height jump.")]
        public float jumpVariableHeightGravityMultiplier = 1.8f;
    }
}