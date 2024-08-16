using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyScripts.EnemyBehaviors {
    public class PatrollingBehavior : MonoBehaviour,IEnemyBehavior
    {
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private float speed = 1f;
        
        public IEnumerator StartMovement() {
            int targetIndex = 0;
            while (true) {
                Vector3 target = patrolPoints[targetIndex].position;
                while (Vector3.Distance(transform.position, target) > 0.3f) {
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    yield return null;
                }

                targetIndex = (targetIndex + 1) % patrolPoints.Length;
            }
        }
    }
}
