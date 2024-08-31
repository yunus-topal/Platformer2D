using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnemyScripts;

public class HeroKnight : MonoBehaviour {

    [Header("General")] 
    [SerializeField] private float      m_hp = 100.0f;
    [SerializeField] private float      m_speed = 4.0f;
    [SerializeField] private float      m_jumpForce = 200f;
    [SerializeField] private float      m_rollForce = 150f;
    [SerializeField] private float      m_knockback = 3f;
    [SerializeField] private bool       m_noBlood = false;
    [SerializeField] private GameObject m_slideDust;
    
    [Header("Attack Section")] 
    [SerializeField] private float m_attackDamage = 10f;
    [SerializeField] private float m_attackCooldown = 0.25f;
    [SerializeField] private GameObject m_attackPointRight;
    [SerializeField] private GameObject m_attackPointLeft;
    [SerializeField] private float m_attackRadius;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;

    // layer mask ids
    private static int _playerLayer;
    private static int _enemyLayer;
    
    // animation hashes
    private static readonly int Hurt = Animator.StringToHash("Hurt");
    private static readonly int WallSlide = Animator.StringToHash("WallSlide");
    private static readonly int AirSpeedY = Animator.StringToHash("AirSpeedY");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int IdleBlock = Animator.StringToHash("IdleBlock");
    private static readonly int Block = Animator.StringToHash("Block");
    private static readonly int AnimState = Animator.StringToHash("AnimState");


    void Awake() {
        _playerLayer = LayerMask.NameToLayer("Player");
        _enemyLayer = LayerMask.NameToLayer("Enemy");
    }
    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update ()
    {
        #region stateLogic

        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        #endregion

        #region movement

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling )
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat(AirSpeedY, m_body2d.velocity.y);

        #endregion



        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool(WallSlide, m_isWallSliding);
 
        /*
        //Death
        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
            
        //Hurt
        else if (Input.GetKeyDown("q") && !m_rolling)
            m_animator.SetTrigger("Hurt");
        
        */
        
        //Attack
        if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > m_attackCooldown && !m_rolling)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);
            Invoke(nameof(Attack), 0.1f);
            
            // Reset timer
            m_timeSinceAttack = 0.0f;
        }
        
        //Jump
        else if (Input.GetKeyDown("w") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger(Jump);
            m_grounded = false;
            m_animator.SetBool(Grounded, m_grounded);
            m_body2d.AddForce(new Vector2(0f, m_jumpForce));
            m_groundSensor.Disable(0.2f);
        }
        /*
        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            //m_body2d.AddForce(new Vector2(m_facingDirection * m_rollForce,0f), ForceMode2D.Impulse);
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }
        */
        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger(Block);
            m_animator.SetBool(IdleBlock, true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool(IdleBlock, false);

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger(AnimState, 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger(AnimState, 0);
        }
    }

    public void Attack() {
        Collider2D[] collisions;
        if(m_facingDirection == 1) collisions = Physics2D.OverlapCircleAll(m_attackPointRight.transform.position, m_attackRadius);
        else collisions = Physics2D.OverlapCircleAll(m_attackPointLeft.transform.position, m_attackRadius);

        foreach (var collision in collisions) {
            if (collision.gameObject.CompareTag("Enemy")) {
                // call enemy damage function.
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(m_attackDamage, transform.position);
            }
        }
    }
    
    public void TakeDamage(float damage, Vector3 position) {
        m_hp -= damage;
        if (m_hp <= 0) {
            Destroy(gameObject);
            return;
        }       
        m_animator.SetTrigger(Hurt);
        
        // todo: fix this. It's not working because of input reading at the beggining of the update loop.
        //var target = transform.position;
        // apply knockback depending on the position of the hit. Just check x direction
        if (position.x > transform.position.x) {
            Debug.Log("AAAAAAAAAAA");
            //target.x -= m_knockback;
            m_body2d.velocity = new Vector2(m_knockback, m_body2d.velocity.y);
            //transform.position = Vector3.MoveTowards(transform.position, target, m_knockback);
        }
        else {
            Debug.Log("BBBBBBBBBB");

            //target.x += m_knockback;
            //transform.position = Vector3.MoveTowards(transform.position, target, m_knockback);
            m_body2d.velocity = new Vector2(m_knockback, m_body2d.velocity.y);
        }
        
            
        // todo: find a better way for this.
        Physics2D.IgnoreLayerCollision(_playerLayer,_enemyLayer,true);
        // _collider2D.excludeLayers = enemyLayers;
            
        Invoke(nameof(HitRecover), 1f);
    }
    
    public void HitRecover() {
        List<Collider2D> collisions = new();
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        // TODO: actually use contact filter.
        gameObject.GetComponent<Collider2D>().OverlapCollider(contactFilter2D.NoFilter(),collisions);

        if (collisions.Any(collision => collision.gameObject.CompareTag("Enemy"))) {
            Debug.Log("enemy still in contact.");
            TakeDamage(1f, collisions.First().transform.position);
            return;
        }
            
        Debug.Log("player recovered");
            
        Physics2D.IgnoreLayerCollision(_playerLayer,_enemyLayer,false);
    }
    
    // Which is better: Handle in player script or in enemy script?
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            TakeDamage(1f, other.gameObject.transform.position);
        }
    }
    
    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}
