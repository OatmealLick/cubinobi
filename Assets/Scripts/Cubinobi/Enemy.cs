using ModestTree;
using UnityEngine;

namespace Cubinobi
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        protected int health;
        
        [SerializeField]
        protected Rigidbody2D _rigidbody2D;

        [SerializeField]
        protected PlayerController _playerController;

        protected LayerMask playerCharacterMask;
        protected LayerMask collisionMask;

        [SerializeField]
        protected SpriteRenderer _spriteRenderer;

        [SerializeField]
        protected bool facingRight = false;

        protected readonly Effects _effects = new();

        protected virtual void Start()
        {
            playerCharacterMask = LayerMask.GetMask("Default");
            collisionMask = LayerMask.GetMask("Enemies", "LevelGeometry");
        }

        protected virtual void Update()
        {
            Assert.That(_playerController != null);
            _effects.Update();
        }

        public void TakeDamage(int damage = 0, HitEffects hitEffects = null)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
                return;
            }

            if (hitEffects != null)
            {
                if (hitEffects.isStunning)
                {
                    Stun(hitEffects.stunDuration);
                }
            }
        }

        protected void Stun(float duration)
        {
            _rigidbody2D.velocity = Vector2.zero;
            _effects.Add(HitEffect.Stun, duration);
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        protected void SwitchDirection()
        {
            facingRight = !facingRight;
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }

        protected Vector2 Direction()
        {
            return facingRight ? Vector2.right : Vector2.left;
        }
    }
    public enum HitEffect
    {
        Stun = 0,
    }
}