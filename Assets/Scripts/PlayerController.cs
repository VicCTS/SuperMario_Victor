using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int direction = 1;

    private float inputHorizontal;

    private Rigidbody2D rigidBody;
    private GroundSensor _groundSensor;
    private SpriteRenderer _spriteRender;
    private Animator _animator;
    private AudioSource _audioSource;

    public float playerSpeed = 4.5f;
    public float jumpForce = 10f;

    public AudioClip jumpSFX;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        _groundSensor = GetComponentInChildren<GroundSensor>();
        _spriteRender = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
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
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump") && _groundSensor.isGrounded)
        {
            Jump();
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
            _spriteRender.flipX = false;
            _animator.SetBool("IsRunning", true);
        }
        else if(inputHorizontal < 0)
        {
            _spriteRender.flipX = true;
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
}
