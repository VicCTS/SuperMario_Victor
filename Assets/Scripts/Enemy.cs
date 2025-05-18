using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Slider _healthBar;
    private Animator _animator;
    private AudioSource _audioSource;
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;
    private GameManager _gameManager;

    public int direction = 1;
    public float speed = 5;

    public AudioClip _deathSFX;

    public float maxHealth = 5;
    private float currentHealth;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _healthBar = GetComponentInChildren<Slider>();
        _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        _healthBar.maxValue = maxHealth;
        _healthBar.value = maxHealth;
    }

    void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2(direction * speed, _rigidBody.velocity.y);
    }

    public void Death()
    {
        direction = 0;
        _rigidBody.gravityScale = 0;
        _animator.SetTrigger("IsDead");
        _audioSource.PlayOneShot(_deathSFX);
        _boxCollider.enabled = false;
        Destroy(gameObject, 0.3f);
    }

    public void TakeDamage(float damage)
    {
        currentHealth-= (int)damage;

        _healthBar.value = currentHealth;

        if(currentHealth <= 0)
        {
            Death();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Tuberia") || collision.gameObject.layer == 6)
        {
            direction *= -1;
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            //Destroy(collision.gameObject);
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            playerScript.Death();
        }
    }

    void OnBecameVisible()
    {
        direction = 1;
        _gameManager.enemiesInScreen.Add(gameObject);
    }

    void OnBecameInvisible()
    {
        direction = 0;
        _gameManager.enemiesInScreen.Remove(gameObject);
    }
}
