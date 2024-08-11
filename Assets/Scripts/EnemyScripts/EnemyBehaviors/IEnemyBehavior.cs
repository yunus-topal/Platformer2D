using System.Collections;
using UnityEngine;

namespace EnemyScripts.EnemyBehaviors {
    public interface IEnemyBehavior {
        public IEnumerator StartMovement();
    }
}
