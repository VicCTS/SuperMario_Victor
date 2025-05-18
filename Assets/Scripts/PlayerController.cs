using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{    
    private Rigidbody2D rigidBody;
    private GroundSensor _groundSensor;
    private SpriteRenderer _spriteRender;
    private Animator _animator;
    private AudioSource _audioSource;
    private BoxCollider2D _boxCollider;
    private GameManager _gameManager;
    private SoundManager _soundManager;

    private float inputHorizontal;

    public float playerSpeed = 4.5f;
    public float jumpForce = 10f;

    [SerializeField] private float _dashForce = 20;
    [SerializeField] private float _dashDuration = 0.5f;
    [SerializeField] private float _dashCoolDown = 1;
    private bool _canDash = true;
    private bool _isDashing = false;

    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _attackDamage = 10;
    [SerializeField] private float _attackRadius = 1;
    [SerializeField] private Transform _hitBoxPosition;

    [SerializeField] private float _baseChargedAttackDamage = 15;
    [SerializeField] private float _maxChargedAttackDamage = 40;
    private float _chargedAttackDamage;

    public Transform bulletSpawn;
    public GameObject bulletPrefab;
    public bool canShoot = false;
    public float powerUpDuration = 10f;
    public float powerUpTimer;
    public Image powerUpImage;

    public AudioClip jumpSFX;
    public AudioClip deathSFX;
    public AudioClip shootSFX;

    private HingeJoint2D _joint;
    private bool _isJoint = false;

    void Awake()    
    {
        rigidBody = GetComponent<Rigidbody2D>();
        _groundSensor = GetComponentInChildren<GroundSensor>();
        _spriteRender = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _soundManager = FindObjectOfType<SoundManager>().GetComponent<SoundManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Esto teletransporta al personaje
        //transform.position = new Vector3(-82.7f, -1.5f, 0); 

        _chargedAttackDamage = _baseChargedAttackDamage;  
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.G))
        {
            direction();
        }
        if(!_gameManager.isPlaying)
        {
            return;
        }

        if(_gameManager.isPaused)
        {
            return;
        }

        if(_isDashing)
        {
            return;
        }

        inputHorizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            if(_groundSensor.isGrounded || _groundSensor.canDoubleJump)
            {
                Jump();
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
        {
            StartCoroutine(Dash());
        }

        if(Input.GetButtonDown("Fire2"))
        {
            NormalAttack();
        }

        /*if(Input.GetButton("Fire2"))
        {
            AttackCharge();
        }

        if(Input.GetButtonUp("Fire2"))
        {
            ChargedAttack();
        }*/

        if(Input.GetButtonDown("Fire1") && canShoot)
        {
            Shoot();
        }

        if(canShoot)
        {
            PowerUp();
        }

        Movement();

        _animator.SetBool("IsJumping", !_groundSensor.isGrounded);

        if(Input.GetKeyDown(KeyCode.R))
        {
            GrabJoint();
        }

        /*if(_groundSensor.isGrounded)
        {
            _animator.SetBool("IsJumping", false);
        }
        else
        {
            _animator.SetBool("IsJumping", true);
        }*/

        //transform.position = new Vector3(transform.position.x + direction * playerSpeed * Time.deltaTime, transform.position.y, transform.position.z) ;
        //transform.Translate(new Vector3(direction * playerSpeed * Time.deltaTime, 0, 0));
        //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + inputHorizontal, transform.position.y), playerSpeed * Time.deltaTime);

    }

    void FixedUpdate()
    {
        if(_isDashing)
        {
            return;
        }

        if(!_isJoint)
        {
            rigidBody.velocity = new Vector2(inputHorizontal * playerSpeed, rigidBody.velocity.y);
        }

        if(inputHorizontal != 0 && _isJoint)
        {
            rigidBody.AddForce(new Vector2(inputHorizontal * playerSpeed * 2, 0));
        }

        //rigidBody.velocity = new Vector2(inputHorizontal * playerSpeed, rigidBody.velocity.y);
        //rigidBody.AddForce(new Vector2(inputHorizontal, 0));
        //rigidBody.MovePosition(new Vector2(100, 0));
    }

    void Movement()
    {
        if(inputHorizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _animator.SetBool("IsRunning", true);
        }
        else if(inputHorizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }
    }

    void Jump()
    {
        if(!_groundSensor.isGrounded)
        {
            _groundSensor.canDoubleJump = false;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            //_animator.SetBool("doble", true);
            //rigidBody.AddForce(Vector2.up * doblueJumpForce, ForceMode2D.Impulse);
        }
        else
        {
            _animator.SetBool("IsJumping", true);
            //rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _audioSource.PlayOneShot(jumpSFX);
    }

    IEnumerator Dash()
    {
        float gravity = rigidBody.gravityScale;
        rigidBody.gravityScale = 0;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);

        _isDashing = true;
        _canDash = false;
        rigidBody.AddForce(transform.right * _dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(_dashDuration);
        rigidBody.gravityScale = gravity;
        _isDashing = false;

        yield return new WaitForSeconds(_dashCoolDown);
        _canDash = true;
    }

    void NormalAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_hitBoxPosition.position, _attackRadius, _enemyLayer);

        foreach(Collider2D enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.TakeDamage(_attackDamage);
        }
    }

    void AttackCharge()
    {
        if(_chargedAttackDamage < _maxChargedAttackDamage)
        {
            _chargedAttackDamage += Time.deltaTime;
            Debug.Log(_chargedAttackDamage);
        }
        else
        {
            _chargedAttackDamage = _maxChargedAttackDamage;
        }
    }

    void ChargedAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_hitBoxPosition.position, _attackRadius, _enemyLayer);

        foreach(Collider2D enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.TakeDamage(_chargedAttackDamage);
        }

        _chargedAttackDamage = _baseChargedAttackDamage;
    }

    public void Death()
    {
        _animator.SetTrigger("IsDead");
        _audioSource.PlayOneShot(deathSFX);
        _boxCollider.enabled = false;

        Destroy(_groundSensor.gameObject);

        inputHorizontal = 0;
        rigidBody.velocity = Vector2.zero;

        rigidBody.AddForce(Vector2.up * jumpForce / 2, ForceMode2D.Impulse);

        //_soundManager.Invoke("DeathBGM", deathSFX.length);
        StartCoroutine(_soundManager.DeathBGM());
        //_soundManager.StartCoroutine("DeathBGM");

        _gameManager.isPlaying = false;

        Destroy(gameObject, 2);
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        _audioSource.PlayOneShot(shootSFX);
    }

    void PowerUp()
    {
        powerUpTimer += Time.deltaTime;

        powerUpImage.fillAmount = Mathf.InverseLerp(powerUpDuration, 0, powerUpTimer);

        if(powerUpTimer >= powerUpDuration)
        {
            canShoot = false;
            powerUpTimer = 0;
        }
    }

    void GrabJoint()
    {
        if(!_isJoint)
        {
            Rigidbody2D joint = Physics2D.OverlapCircle(_hitBoxPosition.position, _attackRadius).GetComponent<Rigidbody2D>();
            
            /*if(joint != null)
            {
                HingeJoint2D fj = transform.gameObject.AddComponent(typeof(HingeJoint2D)) as HingeJoint2D;
                fj.connectedBody = joint;
            }*/

            if(joint == null)
            {
                return;
            }

            _joint = gameObject.AddComponent<HingeJoint2D>();
            _joint.connectedBody = joint;

            _isJoint = true;
        }
        else
        {
            Destroy(_joint);
            _isJoint = false;
        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(_hitBoxPosition.position, _attackRadius);
    }

    public float attackRange;
    public Transform mono;

    void direction()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, mono.position);
        Vector3 directionToPlayer = mono.position - transform.position;

        Debug.Log(directionToPlayer);

        
    }


    void Attack()
    {
        //cosas ataque
    }
}
