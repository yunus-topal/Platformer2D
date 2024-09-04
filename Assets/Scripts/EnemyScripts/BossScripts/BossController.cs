using Interfaces;
using UnityEngine;

namespace EnemyScripts.BossScripts {
    // Theoretically, bosses can have multiple attacks and behaviors.
    // Each attack and behavior can be on different scripts, but they all should be controlled from the BossController.
    // Or there might be special attack and behavior scripts with multiple patterns.
    public class BossController : MonoBehaviour, IDamageable
    {
        [SerializeField] private Enemy enemyInfo;

        private float _currentHp = 1f;
        private Animator _animator;

        private void Start() {
            _animator = GetComponent<Animator>();
            _currentHp = enemyInfo.Hp;
        }

        public void TakeDamage(float damage, Vector3 position) {
            _currentHp -= damage;

            if (_currentHp <= 0f) {
                Die();
            }
        }

        private void Die() {
            Destroy(gameObject);
        }
    }
}
