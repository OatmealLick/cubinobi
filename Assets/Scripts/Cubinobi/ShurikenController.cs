using Sirenix.OdinInspector;
using UnityEngine;

namespace Cubinobi
{
    public class ShurikenController : MonoBehaviour
    {
        [SerializeField]
        [HideIn(PrefabKind.PrefabInstance)]
        private Rigidbody2D _rigidbody2D;
        
        private LayerMask _enemiesMask;
        private LayerMask _levelGeometryMask;
        private float _timer;
        private float _maxTime;
        private int _damage;
        private HitEffects _effects;

        public void Setup(float rangedAttackSpeed,
            Vector2 direction,
            float hitboxLength,
            int damage,
            HitEffects effects,
            LayerMask enemiesMask,
            LayerMask levelGeometryMask)
        {
            _enemiesMask = enemiesMask;
            _levelGeometryMask = levelGeometryMask;
            _damage = damage;
            _effects = effects;

            _rigidbody2D.velocity = direction * rangedAttackSpeed;
            _timer = 0f;
            _maxTime = hitboxLength / rangedAttackSpeed;
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
                col.gameObject.GetComponent<Enemy>().TakeDamage(_damage, _effects);
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