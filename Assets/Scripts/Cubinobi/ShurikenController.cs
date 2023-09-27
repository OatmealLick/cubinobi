using System;
using System.Timers;
using Cubinobi.Project;
using UnityEngine;

namespace Cubinobi
{
    public class ShurikenController : MonoBehaviour
    {
        private LayerMask _enemiesMask;
        private LayerMask _levelGeometryMask;
        private float _timer;
        private float _maxTime;

        [SerializeField]
        private Rigidbody2D _rigidbody2D;

        public void Setup(StanceSettings stanceSettings,
            Vector2 direction,
            float hitboxLength,
            LayerMask enemiesMask,
            LayerMask levelGeometryMask)
        {
            _enemiesMask = enemiesMask;
            _levelGeometryMask = levelGeometryMask;

            _rigidbody2D.velocity = direction * stanceSettings.rangedAttackSpeed;
            _timer = 0f;
            _maxTime = hitboxLength / stanceSettings.rangedAttackSpeed;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _maxTime)
            {
                Die();
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_enemiesMask.Contains(col.gameObject.layer))
            {
                col.gameObject.GetComponent<Enemy>().Hit();
                Die();
            }
            else if (_levelGeometryMask.Contains(col.gameObject.layer))
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}