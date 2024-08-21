using System;
using UnityEngine;

namespace ManagerScripts {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject bossBlockage;

        private BoxCollider2D _bossAreaCollider;

        private void Start() {
            _bossAreaCollider = GetComponent<BoxCollider2D>();
        }

        // boss area trigger
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.gameObject.CompareTag("Player")) return;

            _bossAreaCollider.enabled = false;
            // kill common mobs.
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemies.Length; i++) {
                Destroy(enemies[i]);
            }
            // block the entrance.
            bossBlockage.SetActive(true);
            // initiate boss fight.
            SpawnBoss();
        }

        private void SpawnBoss() {
            
        }
    }
}
