using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyScripts {
    [CreateAssetMenu(fileName = "enemy", menuName = "Enemy")]
    public class Enemy : ScriptableObject {
        [SerializeField] private new string name;
        [SerializeField] private float hp;
        [SerializeField] private float collisionDamage;

        public string Name => name;

        public float Hp => hp;

        public float CollisionDamage => collisionDamage;
    }
}
