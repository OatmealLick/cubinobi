using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cubinobi.Project
{
    [CreateAssetMenu(fileName = "ElementalStancesResources", menuName = "Cubinobi/ElementalStancesResources", order = 2)]
    public class ElementalStancesResources : SerializedScriptableObject
    {
        [Title("Static Resources")]
        public Dictionary<ElementalStance, ElementalStanceData> Data = new();

        [Serializable]
        public struct ElementalStanceData
        {
            public Color CharacterIndicatorColor;
        }
    }
    
}