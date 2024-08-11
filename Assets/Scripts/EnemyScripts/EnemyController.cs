using System;
using EnemyScripts.EnemyBehaviors;
using UnityEngine;

namespace EnemyScripts {
    public class EnemyController : MonoBehaviour {
        private IEnemyBehavior _enemyBehavior;
        private void Start() { 
            _enemyBehavior = GetComponent<IEnemyBehavior>();
            StartCoroutine(_enemyBehavior.StartMovement());
        }
    }
}
