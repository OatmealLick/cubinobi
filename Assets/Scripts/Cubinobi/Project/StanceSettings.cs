using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cubinobi.Project
{
    [CreateAssetMenu(fileName = "StanceSettings", menuName = "Cubinobi/StanceSettings", order = 0)]
    public class StanceSettings : ScriptableObject
    {
        [Title("Movement Parameters")]
        public float movementSpeed = 6.0f;

        [Title("Jump Parameters")]
        [Range(1, 10), InfoBox("How high the character can reach in the peak of the jump")]
        public float jumpHeight = 5.0f;

        [Range(0.1f, 1.0f), InfoBox("Time it takes to reach the peak height of the jump.")]
        public float jumpTimeToPeak = 0.4f;

        [Range(1.0f, 2.0f),
         InfoBox("When character is falling down this multiplies the gravity to achieve Hollow Knight / Mario effect.")]
        public float jumpFallingGravityMultiplier = 1.4f;

        [InfoBox(
            "Variable jump gravity multiplier. When player releases the jump key before reaching peak, this multiplies the gravity to achieve variable height jump.")]
        public float jumpVariableHeightGravityMultiplier = 1.8f;

        [Title("Melee Attack Parameters")]
        
        [InfoBox("Order right, down, left, up. Position Offset from the center of Sprite. Visible in play mode.")]
        public AttackHitbox[] MeleeAttackHitboxes = new AttackHitbox[4];

        [Title("Ranged Attack Parameters")]
        [Range(0.1f, 200)]
        public float rangedAttackSpeed = 50;
        
        [InfoBox("Order right, down, left, up. Position Offset from the center of Sprite. Visible in play mode.")]
        public AttackHitbox[] RangedAttackHitboxes = new AttackHitbox[4];
    }

    [Serializable]
    public struct AttackHitbox
    {
        public Vector2 Size;
        public Vector2 PositionOffset;
    }
}