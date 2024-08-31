using System;
using EnemyScripts.EnemyAttacks;
using EnemyScripts.EnemyBehaviors;
using UnityEngine;

namespace EnemyScripts {
    [RequireComponent(typeof(Animator))]
    public class EnemyController : MonoBehaviour {
        [SerializeField] private Enemy enemyInfo;

        private float _currentHp = 1f;
        private Animator _animator;
        private static readonly int HitTrig = Animator.StringToHash("hit_trig");

        private void Start() {
            _animator = GetComponent<Animator>();
            _currentHp = enemyInfo.Hp;
        }

        public void TakeDamage(float damage, Vector3 position) {
            _currentHp -= damage;

            if (_currentHp <= 0f) {
                Die();
            }
            else {
                _animator.SetTrigger(HitTrig);
                // apply knockback depending on the position of the hit. Just check x direction
                if (position.x > transform.position.x) {
                    GetComponent<Rigidbody2D>().AddForce(-enemyInfo.KnockbackForce, ForceMode2D.Impulse);
                }
                else {
                    GetComponent<Rigidbody2D>().AddForce(enemyInfo.KnockbackForce, ForceMode2D.Impulse);
                }
            }
        }

        private void Die() {
            Destroy(gameObject);
        }
    }
}
