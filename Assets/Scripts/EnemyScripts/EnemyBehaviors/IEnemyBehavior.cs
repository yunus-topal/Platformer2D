using System.Collections;
using EnemyScripts;
using UnityEngine;

namespace EnemyScripts.EnemyBehaviors {
    public interface IEnemyBehavior {
        public IEnumerator StartMovement();
    }
}
