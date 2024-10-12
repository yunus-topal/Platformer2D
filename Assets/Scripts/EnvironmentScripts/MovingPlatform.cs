using System.Collections;
using UnityEngine;

namespace EnvironmentScripts {
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float speed = 1f;
        [SerializeField] private bool loop = true;

        private GameObject _player;
        private int _currentWaypointIndex;
        private bool _playerOnPlatform;
        
        private void Start()
        {
            _playerOnPlatform = false;
            _currentWaypointIndex = 0;
            if(waypoints.Length == 0) return;
            _player = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(MovePlatform());
        }

        public void SetPlayerOnPlatform(bool value)
        {
            _playerOnPlatform = value;
            if (_playerOnPlatform)
            {
                _player.transform.SetParent(transform);
            }
            else
            {
                _player.transform.SetParent(null);
            }
        }
        
        private IEnumerator MovePlatform()
        {
            do {
                var target = waypoints[_currentWaypointIndex].position;
                while (transform.position != target) {
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    yield return null;
                }

                _currentWaypointIndex++;
                _currentWaypointIndex %= waypoints.Length;
            } while (loop);
        }
    }
}
