using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace EnemyScripts.EnemyAttacks {
    [RequireComponent(typeof(EnemyController))]
    public class ArtilleryAttack : MonoBehaviour, IEnemyAttack {
        [SerializeField] private GameObject artilleryShell;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float shellForce = 10f;
        [SerializeField] private float attackCooldown = 1f;
        
        private EnemyController _enemyController;

        private void Awake() {
            _enemyController = GetComponent<EnemyController>();
        }

        public void StartAttack() {
            StartCoroutine(StartArtillery());
        }

        private IEnumerator StartArtillery() {
            while (true) {
                var bullet = Instantiate(artilleryShell, attackPoint.position, Quaternion.identity);
                // random force on either direction.
                // remove collisions with everything except player.
                float horizontalForce = Random.Range(-shellForce / 2 , shellForce / 2);
                
                bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(horizontalForce, shellForce), ForceMode2D.Impulse);
                
                yield return new WaitForSeconds(attackCooldown);
            }
        }

    }
}
