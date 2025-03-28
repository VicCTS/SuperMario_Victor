using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private CircleCollider2D _collider;
    private AudioSource _audioSource;
    private SpriteRenderer _renderer;

    private int direction = 1;
    public float speed = 2.5f;

    public AudioClip powerUpSFX;


    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2(direction * speed, _rigidBody.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Tuberia"))
        {
            direction *= -1;
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            playerScript.canShoot = true;
            Interact();
        }
    }

    void Interact()
    {
        direction = 0;
        _rigidBody.gravityScale = 0;
        _collider.enabled = false;
        _renderer.enabled = false;
        _audioSource.PlayOneShot(powerUpSFX);
        Destroy(gameObject, 0.5f);
    }
}
