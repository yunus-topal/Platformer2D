using System;
using UnityEngine;

namespace PlayerScripts {
    public class PlayerController : MonoBehaviour {

        [SerializeField] private float attackCooldown = 1.0f;
        private float _lastAttack = float.MinValue;
        
        private CharacterController2D _cc;
        private Animator _animator;

        public float speed = 10f;


        private void Start() {
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
            bool crouch = Input.GetButton("Crouch");

            _cc.Move(move * speed, crouch, jump);

            
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time - _lastAttack > attackCooldown) {
                _animator.SetTrigger("attack_trig");
                _lastAttack = Time.time;
            }        
        }
    }
}