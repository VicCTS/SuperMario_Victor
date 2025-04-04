using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    BoxCollider2D _collider;
    AudioSource _audioSource;
    SpriteRenderer _renderer;
    GameManager _gameManager;

    public AudioClip sfx;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            TakeCoin();
        }
    }

    void TakeCoin()
    {
        _gameManager.AddCoins();

        _collider.enabled = false;
        _renderer.enabled = false;
        _audioSource.PlayOneShot(sfx);
        Destroy(gameObject, sfx.length);
    }
}
