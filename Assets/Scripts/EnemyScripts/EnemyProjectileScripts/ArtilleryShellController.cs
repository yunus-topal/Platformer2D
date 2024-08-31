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
                other.gameObject.GetComponent<HeroKnight>().TakeDamage(damage, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
