using System;
using System.Collections;
using UnityEngine;

namespace EnemyScripts.EnemyBehaviors {
    public class GhostBehavior : MonoBehaviour {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float oscillationFrequency = 2f; // Frequency of oscillation
        [SerializeField] private float oscillationMagnitude = 2f; // Magnitude of oscillation
        private GameObject _player;


        private void Start() {
            StartCoroutine(StartMovement());
        }

        public IEnumerator StartMovement() {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null) yield break;

            while (true) {
                var target = _player.transform.position;
                target.y += 1.0f;

                if ((target - transform.position).magnitude < 25f) {
                    // Calculate the offset using sine wave
                    float offsetX = Mathf.Sin(Time.time * oscillationFrequency) * oscillationMagnitude;
                    float offsetZ = Mathf.Cos(Time.time * oscillationFrequency) * oscillationMagnitude;

                    // Create the oscillation offset vector
                    Vector3 oscillationOffset = new Vector3(offsetX, 0, offsetZ);

                    // Move towards the target with the oscillation offset
                    transform.position = Vector3.MoveTowards(transform.position, target + oscillationOffset, speed * Time.deltaTime);
                }

                yield return null;
            }
        }

    }
}
