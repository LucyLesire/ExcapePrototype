using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    private float _totalTime;
    private bool _timing;

    private string _gameOverLabel;
    public string GameOverLabel
    {
        get { return _gameOverLabel; }
        set { _gameOverLabel = value; }
    }

    private int _totalZombies;
    public int TotalZombies
    {
        get { return _totalZombies; }
        set { _totalZombies = value; }
    }

    private int _totalAmmo;
    public int TotalAmmo
    {
        get { return _totalAmmo; }
        set { _totalAmmo = value; }
    }

    private int _totalHearts;
    public int TotalHearts
    {
        get { return _totalHearts; }
        set { _totalHearts = value; }
    }

    private int _totalWaves;
    public int TotalWaves
    {
        get { return _totalWaves; }
        set { _totalWaves = value; }
    }
    public float TotalTime
    {
        get { return _totalTime; }
    }

    public bool Timer
    {
        get { return _timing; }
        set { _timing = value; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        if(_timing)
        {
            _totalTime += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        _timing = true;
    }
}
