using System.Collections;
using UnityEngine;

namespace EnemyScripts.EnemyBehaviors {
    [RequireComponent(typeof(EnemyController))]
    public class PatrollingBehavior : MonoBehaviour,IEnemyBehavior
    {
        [SerializeField] private Transform[] _patrolPoints;
        [SerializeField] private float _speed = 1f;
        
        public IEnumerator StartMovement() {
            int targetIndex = 0;
            while (true) {
                Vector3 target = _patrolPoints[targetIndex].position;
                while (Vector3.Distance(transform.position, target) > 0.3f) {
                    transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
                    yield return null;
                }

                targetIndex = (targetIndex + 1) % _patrolPoints.Length;
            }
        }
    }
}
