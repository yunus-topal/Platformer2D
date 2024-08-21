using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyScripts.EnemyAttacks {
    public class ArtilleryAttack : MonoBehaviour {
        [SerializeField] private GameObject artilleryShell;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float shellForce = 10f;
        [SerializeField] private float attackCooldown = 1f;
        
        private GameObject _player;
        
        private void Start() {
            StartCoroutine(StartArtillery());
        }

        private IEnumerator StartArtillery() {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null) yield break;
            
            while (true) {
                var target = _player.transform.position;
                while ((target - transform.position).magnitude > 25f) {
                    yield return new WaitForSeconds(0.5f);
                }
                
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
