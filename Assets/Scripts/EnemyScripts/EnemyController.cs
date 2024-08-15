using System;
using EnemyScripts.EnemyBehaviors;
using UnityEngine;

namespace EnemyScripts {
    public class EnemyController : MonoBehaviour {
        [SerializeField] private Enemy enemyInfo;

        private float _currentHp = 1f;
        
        private IEnemyBehavior _enemyBehavior;
        private void Start() { 
            _enemyBehavior = GetComponent<IEnemyBehavior>();
            StartCoroutine(_enemyBehavior.StartMovement());

            _currentHp = enemyInfo.Hp;
        }

        public void TakeDamage(float damage) {
            _currentHp -= damage;

            if (_currentHp <= 0f) {
                Die();
            }
        }

        private void Die() {
            Destroy(this);
        }
    }
}
