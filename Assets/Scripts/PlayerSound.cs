using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _footStepsAudio;
    private GroundSensor _groundSensor;
    private ParticleSystem _particleSystem;
    private Transform _particlesTransform;
    private Vector3 _particlesPosition;

    private bool _alreadePlaying = false;


    void Awake()
    {
        _groundSensor = GetComponentInChildren<GroundSensor>();  
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _particlesTransform = _particleSystem.transform;     
        _particlesPosition = _particlesTransform.localPosition; 
    }
    // Start is called before the first frame update
    void Start()
    {
        _audioSource.loop = true;
        _audioSource.clip = _footStepsAudio;
    }

    // Update is called once per frame
    void Update()
    {
        FootStepsSound();
    }

    void FootStepsSound()
    {
        if(_groundSensor.isGrounded && Input.GetAxisRaw("Horizontal") != 0 && !_alreadePlaying)
        {
            _particlesTransform.SetParent(gameObject.transform);
            _particlesTransform.localPosition = _particlesPosition;
            _particlesTransform.rotation = transform.rotation;
            _audioSource.Play();
            _particleSystem.Play();
            _alreadePlaying = true;
        }
        else if(!_groundSensor.isGrounded || Input.GetAxisRaw("Horizontal") == 0)
        {
            _particlesTransform.SetParent(null);
            _audioSource.Stop();
            _particleSystem.Stop();
            _alreadePlaying = false;
        }
    }
}
