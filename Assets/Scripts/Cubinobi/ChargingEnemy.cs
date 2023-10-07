using System;
using Cubinobi.Project;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Cubinobi
{
    public class ChargingEnemy : Enemy
    {
        private EnemiesSettings _enemiesSettings;
        private Tween attackingSequence;

        [SerializeField]
        private float patrolingTimer;

        [SerializeField]
        private State _state = State.Patroling;

        private readonly RaycastHit2D[] spottedHits = new RaycastHit2D[1];

        [Inject]
        private void Construct(Settings settings)
        {
            _enemiesSettings = settings.EnemiesSettings;
        }

        protected override void Start()
        {
            base.Start();
            _rigidbody2D.velocity = Direction() * _enemiesSettings.ceMovementSpeed;
            health = _enemiesSettings.ceHealth;
        }

        protected override void Update()
        {
            base.Update();

            if (_effects.Has(HitEffect.Stun))
            {
                if (attackingSequence != null && attackingSequence.IsPlaying())
                {
                    attackingSequence.Kill();
                    _state = State.Patroling;
                    patrolingTimer = _enemiesSettings.ceTimeToPatrolBeforeSwitchingDirections;
                }
                return;
            }

            var hitCount = Physics2D.RaycastNonAlloc(transform.position, Direction(), spottedHits,
                _enemiesSettings.ceReactThePlayerDistance, playerCharacterMask);

            if (hitCount != 0 && _state == State.Patroling)
            {
                _state = State.Charging;

                var anticipationDirection = facingRight ? -1 : 1;
                attackingSequence = DOTween.Sequence()
                    .Append(transform.DOLocalMoveX(transform.localPosition.x + 0.5f * anticipationDirection,
                        _enemiesSettings.ceChargeLoadingTime))
                    .AppendCallback(() => { _rigidbody2D.velocity = Direction() * _enemiesSettings.ceChargingSpeed; })
                    .AppendInterval(_enemiesSettings.ceChargingTime)
                    .AppendCallback(() => { _rigidbody2D.velocity = Vector2.zero; })
                    .AppendInterval(_enemiesSettings.ceRecoveryAfterChargingTime)
                    .AppendCallback(() =>
                    {
                        _rigidbody2D.velocity = Direction() * _enemiesSettings.ceMovementSpeed;
                        _state = State.Patroling;
                        patrolingTimer = _enemiesSettings.ceTimeToPatrolBeforeSwitchingDirections;
                    })
                    .SetEase(Ease.Linear);
            }
            else if (_state == State.Patroling)
            {
                if (patrolingTimer <= 0)
                {
                    patrolingTimer = _enemiesSettings.ceTimeToPatrolBeforeSwitchingDirections;
                    SwitchDirection();
                }
                else
                {
                    patrolingTimer -= Time.deltaTime;
                }

                _rigidbody2D.velocity = Direction() * _enemiesSettings.ceMovementSpeed;
            }
        }

        private void OnDrawGizmos()
        {
            if (_enemiesSettings != null)
            {
                Gizmos.color = new Color(1, 0, 0, 0.3f);
                Gizmos.DrawLine(transform.position,
                    transform.position + (Vector3) (Direction() * _enemiesSettings.ceReactThePlayerDistance));
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (playerCharacterMask.Contains(col.gameObject.layer))
            {
                col.gameObject.GetComponent<PlayerController>().TakeDamage(_enemiesSettings.ceDamage, _enemiesSettings.ceEffects);
            }
            else if (collisionMask.Contains(col.gameObject.layer))
            {
                switch (_state)
                {
                    case State.Patroling:
                        patrolingTimer = _enemiesSettings.ceTimeToPatrolBeforeSwitchingDirections;
                        SwitchDirectionAndMove();
                        break;
                    case State.Charging:
                        attackingSequence.Kill();
                        _state = State.Patroling;
                        patrolingTimer = _enemiesSettings.ceTimeToPatrolBeforeSwitchingDirections;
                        SwitchDirectionAndMove();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SwitchDirectionAndMove()
        {
            SwitchDirection();
            _rigidbody2D.velocity = Direction() * _enemiesSettings.ceMovementSpeed;
        }

        public enum State
        {
            Patroling = 0,
            Charging = 1
        }
    }
}