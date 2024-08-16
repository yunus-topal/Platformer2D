using System;
using PlayerScripts;
using UnityEngine;

namespace EnemyScripts.EnemyProjectileScripts {
    public class ArtilleryShellController : MonoBehaviour {
        
        [SerializeField] private float damage = 10f;
        private void FixedUpdate() {
            if (transform.position.y < -15f) {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("Player")) {
                Debug.Log("hit player");
                other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
