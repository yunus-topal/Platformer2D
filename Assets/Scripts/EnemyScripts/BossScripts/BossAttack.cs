using System.Collections;
using UnityEngine;

namespace EnemyScripts.BossScripts {
    public class BossAttack : MonoBehaviour
    {
        [SerializeField] private float attackCooldown = 2f;
        [SerializeField] private Transform leftJumpPoint;
        [SerializeField] private Transform rightJumpPoint;
        
        [SerializeField] private float jumpHeight = 5f;
        [SerializeField] private float jumpDuration = 1f;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(BossAttacks());
        }

        private IEnumerator BossAttacks() {
            while (true) {
                yield return new WaitForSeconds(attackCooldown);
                // jump one side of the pit and attack with a horizontal slash.
                Vector3 targetPosition = leftJumpPoint.position;
                if (transform.localPosition.x < 0) {
                    Debug.Log("yippie");
                    targetPosition = rightJumpPoint.position;
                }
         
                // make a parabolic jump to the target position.
                float jumpTime = 0f;
                while (jumpTime < jumpDuration) {
                    jumpTime += Time.deltaTime;
                    float x = Mathf.Lerp(transform.position.x, targetPosition.x, jumpTime / jumpDuration);
                    float y = Mathf.Lerp(transform.position.y, targetPosition.y + jumpHeight, jumpTime / jumpDuration);
                    transform.position = new Vector3(x, y, 0f);
                    yield return null;
                }
            }
        }
    }
}
