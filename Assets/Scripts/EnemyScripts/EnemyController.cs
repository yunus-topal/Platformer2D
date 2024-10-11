using System;
using EnemyScripts.EnemyAttacks;
using EnemyScripts.EnemyBehaviors;
using Interfaces;
using ManagerScripts;
using UnityEngine;

namespace EnemyScripts {
    [RequireComponent(typeof(Animator))]
    public class EnemyController : MonoBehaviour, IDamageable {
        [SerializeField] private Enemy enemyInfo;

        private LevelManager _levelManager;

        private float _currentHp = 1f;
        private Animator _animator;
        private static readonly int HitTrig = Animator.StringToHash("hit_trig");

        private void Start() {
            _animator = GetComponent<Animator>();
            _levelManager = FindObjectOfType<LevelManager>();
            _currentHp = enemyInfo.Hp;
        }

        public void TakeDamage(float damage, Vector3 position) {
            _currentHp -= damage;

            if (_currentHp <= 0f) {
                Die();
            }
            else {
                _animator.SetTrigger(HitTrig);
                var target = transform.position;
                // apply knockback depending on the position of the hit. Just check x direction
                if (position.x > transform.position.x) {
                    target.x -= enemyInfo.Knockback;

                    transform.position = Vector3.MoveTowards(transform.position, target, enemyInfo.Knockback);
                }
                else {
                    target.x += enemyInfo.Knockback;
                    transform.position = Vector3.MoveTowards(transform.position, target, enemyInfo.Knockback);
                }
            }
        }

        private void Die() {
            _levelManager.AddGold(enemyInfo.GoldDrop);
            Destroy(gameObject);
        }
    }
}
