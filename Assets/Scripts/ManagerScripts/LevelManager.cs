using System;
using UnityEngine;

namespace ManagerScripts {
    public class LevelManager : MonoBehaviour {
        public enum GameState {
            Paused,
            Playing,
            GameOver
        }
        
        [SerializeField] private GameObject bossBlockage;
        [SerializeField] private GameObject boss;
        
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject pausePanel;
        
        private GameState _gameState = GameState.Playing;
        private BoxCollider2D _bossAreaCollider;

        private void Start() {
            _bossAreaCollider = GetComponent<BoxCollider2D>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                OnEscPressed();
            }
        }
        
        public void OnEscPressed() {
            if (_gameState == GameState.Paused) {
                Time.timeScale = 1;
                pausePanel.SetActive(false);
                _gameState = GameState.Playing;
            }
            else if(_gameState == GameState.Playing) {
                Time.timeScale = 0;
                pausePanel.SetActive(true);
                _gameState = GameState.Paused;
            }
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
            boss.gameObject.SetActive(true);
        }

        public void GameOver() {
            gameOverPanel.SetActive(true);
        }

        public void QuitGame() {
            Application.Quit();
        }
    }
}
