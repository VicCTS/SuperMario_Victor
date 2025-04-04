using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int direction = 1;

    private float inputHorizontal;

    private Rigidbody2D rigidBody;
    private GroundSensor _groundSensor;
    private SpriteRenderer _spriteRender;
    private Animator _animator;
    private AudioSource _audioSource;
    private BoxCollider2D _boxCollider;
    private GameManager _gameManager;
    private SoundManager _soundManager;

    public float playerSpeed = 4.5f;
    public float jumpForce = 10f;

    public Transform bulletSpawn;
    public GameObject bulletPrefab;
    public bool canShoot = false;
    public float powerUpDuration = 10f;
    public float powerUpTimer;
    public Image powerUpImage;

    public AudioClip jumpSFX;
    public AudioClip deathSFX;
    public AudioClip shootSFX;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(!_gameManager.isPlaying)
        {
            return;
        }

        if(_gameManager.isPaused)
        {
            return;
        }

        inputHorizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump") && _groundSensor.isGrounded)
        {
            Jump();
        }

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
        rigidBody.velocity = new Vector2(inputHorizontal * playerSpeed, rigidBody.velocity.y);
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
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _animator.SetBool("IsJumping", true);
        _audioSource.PlayOneShot(jumpSFX);
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
}
