using System;
using System.Collections.Generic;
using System.Linq;
using EnemyScripts;
using UnityEngine;

namespace PlayerScripts {
    public class PlayerController : MonoBehaviour {

        #region Serialized
        [Header("General")] 
        [SerializeField] private float hp = 100f;
        [SerializeField] private float speed = 10f;

        [Header("Attack Section")] 
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackCooldown = 1.0f;
        [SerializeField] private GameObject attackPoint;
        [SerializeField] private float attackRadius;

        #endregion

        #region private

        private float _lastAttack = float.MinValue;
        private CharacterController2D _cc;
        private Animator _animator;

        // animation hashes
        private static readonly int AttackTrig = Animator.StringToHash("attack_trig");
        private static readonly int HitTrig = Animator.StringToHash("hit_trig");

        // layer mask ids
        private static int _playerLayer;
        private static int _enemyLayer;
        
        #endregion


        private void Awake() {
            _playerLayer = LayerMask.NameToLayer("Player");
            _enemyLayer = LayerMask.NameToLayer("Enemy");
            
            _cc = GetComponent<CharacterController2D>();
            _animator = GetComponent<Animator>();
        }
        
        // Update is called once per frame
        void FixedUpdate() {
            // horizontal movement
            float move = Input.GetAxis("Horizontal");

            // read jump axes from input manager
            bool jump = Input.GetButton("Jump");

            // crouching
            // bool crouch = Input.GetButton("Crouch");

            _cc.Move(move * speed, false, jump);

            
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time - _lastAttack > attackCooldown) {
                _animator.SetTrigger(AttackTrig);
                _lastAttack = Time.time;
            }        
        }

        
        // This function is bound to attack animation. I don't know if this is the best approach but visually makes the most sense.
        public void Attack() {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius);

            foreach (var collision in collisions) {
                if (collision.gameObject.CompareTag("Enemy")) {
                    // call enemy damage function.
                    collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage);
                }
            }
        }

        public void TakeDamage(float damage) {
            Debug.Log("damage taken by player.");
            hp -= damage;
            if (hp <= 0) {
                Die();
                return;
            }
            _animator.SetTrigger(HitTrig);
            
            // todo: find a better way for this.
            Physics2D.IgnoreLayerCollision(_playerLayer,_enemyLayer,true);
            // _collider2D.excludeLayers = enemyLayers;
            
            // TODO: check if there is a way to get animation length instead of a fixed value here.
            Invoke(nameof(HitRecover), 1f);
        }

        // this function WAS bound to hit animation.
        // If you have a transition from "Any State" to an animation and if this function is bound to animation, function call might be skipped which breaks the game.
        public void HitRecover() {
            List<Collider2D> collisions = new();
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            // TODO: actually use contact filter.
            gameObject.GetComponent<Collider2D>().OverlapCollider(contactFilter2D.NoFilter(),collisions);

            if (collisions.Any(collision => collision.gameObject.CompareTag("Enemy"))) {
                Debug.Log("enemy still in contact.");
                TakeDamage(1f);
                return;
            }
            
            Debug.Log("player recovered");
            
            Physics2D.IgnoreLayerCollision(_playerLayer,_enemyLayer,false);
        }

        private void Die() {
            // TODO: create a menu and show it to user.
            Time.timeScale = 0f;
            Destroy(this);
            Debug.Log("You Died.");
        }
        
        // Which is better: Handle in player script or in enemy script?
        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("Enemy")) {
                Debug.Log("enemy collided.");
                TakeDamage(1f);
            }
        }
    }
}