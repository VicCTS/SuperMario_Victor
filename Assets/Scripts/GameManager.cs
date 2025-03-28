using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isPlaying = true;
    public bool isPaused = false;
    private SoundManager _soundManager;

    void Awake()
    {
        _soundManager = FindObjectOfType<SoundManager>().GetComponent<SoundManager>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            Pause();
        }
    }

    void Pause()
    {
        if(isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            _soundManager.PauseBGM();
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
            _soundManager.PauseBGM();
        }
    }
}
