using System;
using EnemyScripts.EnemyAttacks;
using EnemyScripts.EnemyBehaviors;
using UnityEngine;

namespace EnemyScripts {
    [RequireComponent(typeof(Animator))]
    public class EnemyController : MonoBehaviour {
        [SerializeField] private Enemy enemyInfo;
        public Enemy EnemyInfo => enemyInfo;

        private float _currentHp = 1f;
        private IEnemyBehavior _enemyBehavior;
        private IEnemyAttack _enemyAttack;
        private Animator _animator;
        private static readonly int HitTrig = Animator.StringToHash("hit_trig");

        private void Start() {
            _enemyBehavior = GetComponent<IEnemyBehavior>();
            if(_enemyBehavior != null)
                StartCoroutine(_enemyBehavior.StartMovement());
            
            _enemyAttack = GetComponent<IEnemyAttack>();
            if(_enemyAttack != null)
                _enemyAttack.StartAttack();    
            
            _animator = GetComponent<Animator>();
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
