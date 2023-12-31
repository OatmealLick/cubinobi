using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cubinobi.Project
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Cubinobi/Settings", order = 0)]
    public class Settings : SerializedScriptableObject
    {
        [Title("General Settings")]
        [Range(0f, 1f), InfoBox("Some controllers have worn out sticks or are very fragile and pick up on slightest stick movements. This is a deadzone threshold that will be used to filter out the analog stick movements that are below that value.")]
        public float deadzoneInputThreshold = 0.2f;
        
        [InfoBox("Time in seconds for melee attack flash. Purely visual setting, does not effect mechanics. (attack happens instantaneously)")]
        public float attackMeleeGizmoFlashDuration = 0.3f;

        [InfoBox("Initial health of the player.")]
        public int health = 7;

        [InfoBox("Time in seconds for invulnerability after taking damage.")]
        public float invulnerabilityDuration = 1f;

        [Title("Enemies Settings")]
        public EnemiesSettings EnemiesSettings;

        [Title("Stance Settings")]
        [InfoBox("Here each stance gets its own respective set of Settings that can be tweaked per stance.")]
        public Dictionary<ElementalStance, StanceSettings> StanceSettingsMap = new();
    }

    public enum ElementalStance
    {
        Basic = 0,
        Earth = 1,
        Fire = 2,
        Wind = 3,
        Water = 4,
        Void = 5,
    }
}