using System;
using System.Collections;
using UnityEngine;

namespace EnemyScripts.EnemyBehaviors {
    public class GhostBehavior : MonoBehaviour {
        [SerializeField] private float speed = 10f;
        private GameObject _player;


        private void Start() {
            StartCoroutine(StartMovement());
        }

        public IEnumerator StartMovement() {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null) yield break;

            while (true) {
                var target = _player.transform.position;

                if ((target - transform.position).magnitude < 25f) {
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    // TODO add offset to the movement.
                }
                
                yield return null;
            }
        }
    }
}
