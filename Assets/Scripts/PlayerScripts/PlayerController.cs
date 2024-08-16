using System;
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

        [Header("Layer Masks")] 
        [SerializeField] private LayerMask enemyLayers;
        

        #endregion

        #region private

        private float _lastAttack = float.MinValue;
        private CharacterController2D _cc;
        private Animator _animator;
        private CapsuleCollider2D _collider2D;

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
            _collider2D = GetComponent<CapsuleCollider2D>();
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
        }

        // this function is bound to hit animation. I don't know if this is the best approach but visually makes the most sense.
        public void HitRecover() {
            LayerMask contactLayers = gameObject.GetComponent<Collider2D>().contactCaptureLayers;
            if ((contactLayers | enemyLayers) > 0) {
                // TODO: define collision damage.
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