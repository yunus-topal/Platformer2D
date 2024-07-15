using UnityEngine;

namespace PlayerScripts {
    public class PlayerController : MonoBehaviour {

        private Animator _animator;
        private CharacterController2D _cc;

        public float speed = 10f;


        private void Start() {
            _cc = GetComponent<CharacterController2D>();
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void FixedUpdate() {
            // horizontal movement
            float move = Input.GetAxis("Horizontal");
            _animator.SetFloat("move_speed", Mathf.Abs(move));

            // read jump axes from input manager
            bool jump = Input.GetButton("Jump");

            // crouching
            bool crouch = Input.GetButton("Crouch");

            _cc.Move(move * speed, crouch, jump);
        }
    }
}