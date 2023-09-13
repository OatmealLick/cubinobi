using Sirenix.OdinInspector;
using UnityEngine;

namespace Cubinobi.Project
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Cubinobi/Settings", order = 0)]
    public class Settings : ScriptableObject
    {
        [Title("Movement Parameters")]
        public float movementSpeed = 6.0f;
        
        [Title("Jump Parameters", "Out of these parameters all others are computed")]
        [Range(1, 10), InfoBox("How high the character can reach in the peak of the jump")]
        public float jumpHeight = 5.0f;
        
        [Range(0.1f, 1.0f), InfoBox("Time it takes to reach the peak height of the jump.")]
        public float jumpTimeToPeak = 0.4f;

        [Range(1.0f, 2.0f),
         InfoBox("When character is falling down this multiplies the gravity to achieve Hollow Knight / Mario effect.")]
        public float jumpFallingGravityMultiplier = 1.2f;
    }
}