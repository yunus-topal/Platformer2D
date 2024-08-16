using System;
using EnemyScripts.EnemyBehaviors;
using UnityEngine;

namespace EnemyScripts {
    public class EnemyController : MonoBehaviour {
        [SerializeField] private Enemy enemyInfo;

        private float _currentHp = 1f;
        private IEnemyBehavior _enemyBehavior;
        private Animator _animator;
        private static readonly int HitTrig = Animator.StringToHash("hit_trig");

        private void Start() {
            _animator = GetComponent<Animator>();
            _enemyBehavior = GetComponent<IEnemyBehavior>();
            if(_enemyBehavior != null)
                StartCoroutine(_enemyBehavior.StartMovement());

            _currentHp = enemyInfo.Hp;
        }

        public void TakeDamage(float damage) {
            _currentHp -= damage;
            _animator.SetTrigger(HitTrig);
            Debug.Log("damage taken by enemy.");

            if (_currentHp <= 0f) {
                Die();
            }
        }

        private void Die() {
            Destroy(gameObject);
        }
    }
}
