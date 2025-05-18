using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisteryBox : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    public AudioClip _misteryBoxSFX;
    public AudioClip _misteryBoxSFX2;
    
    public GameObject[] powerUpPrefab;
    public int powerUpIndex;
    public Transform powerUPSpawn;

    private bool _isOpen = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void ActivateBox()
    {
        if(!_isOpen)
        {
            _animator.SetTrigger("OpenBox");

            _audioSource.volume = 1f;
            _audioSource.clip = _misteryBoxSFX;

            Instantiate(powerUpPrefab[powerUpIndex], powerUPSpawn.position, powerUPSpawn.rotation);

            _isOpen = true;
        }
        else
        {
            _audioSource.volume = 0.5f;
            _audioSource.clip = _misteryBoxSFX2;
            
        }
        
        _audioSource.Play();
        //_audioSource.Pause();
        //_audioSource.Stop();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            ActivateBox();
        }
    }
}
