using System;
using UnityEngine;

namespace Cubinobi
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private bool isLogging = false;
        
        public void Hit()
        {
            if (isLogging)
            {
                Debug.Log($"I ({gameObject.name}) got hit");
            }
        }
    }
}
