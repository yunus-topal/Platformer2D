using UnityEngine;

namespace EnemyScripts {
    [CreateAssetMenu(fileName = "enemy", menuName = "Enemy")]
    public class Enemy : ScriptableObject {
        [SerializeField] private new string name;
        [SerializeField] private float hp;
        [SerializeField] private float damage;
        [SerializeField] private float attackCooldown;

        public string Name => name;

        public float Hp => hp;

        public float Damage => damage;

        public float AttackCooldown => attackCooldown;
    }
}
