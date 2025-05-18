using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isPlaying = true;
    public bool isPaused = false;
    private SoundManager _soundManager;
    public GameObject pauseCanvas;
    public Text coinsText;

    private int coins = 0;

    public List<GameObject> enemiesInScreen;

    void Awake()
    {
        _soundManager = FindObjectOfType<SoundManager>().GetComponent<SoundManager>();
    }

    void Start()
    {
        coinsText.text = "Coins: " + coins.ToString();
    }

    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            Pause();
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            foreach(GameObject enemy in enemiesInScreen)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.Death();
            }
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        if(isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            _soundManager.PauseBGM();
            pauseCanvas.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
            _soundManager.PauseBGM();
            pauseCanvas.SetActive(true);
        }
    }

    public void AddCoins()
    {
        coins++;
        coinsText.text = "Coins: " + coins.ToString();
    }
}
