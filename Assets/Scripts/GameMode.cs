using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    private float _firstWaveStart = 5.0f;
    [SerializeField]
    private float _waveStartFrequency = 15.0f;
    //[SerializeField]
    //private float _waveEndFrequency = 7.0f;
    //[SerializeField]
    //private float _waveFrequencyIncrement = 0.5f;

    [SerializeField] private GameObject _player = null;

    private float _currentFrequency = 0.0f;
    private const float _waveDelay = 3.0f;

    private int _wave = 0;

    private bool _initiatingWave = true;

    private int _amountOfZombies = 0;

    [SerializeField] private int _maxZombies = 7;

    private void Awake()
    {
        if(_player)
            SpawnManager.Instance.Player = _player;

        _currentFrequency = _waveStartFrequency;

        Invoke(STARTNEWWAVE_METHOD, _firstWaveStart);
    }

    const string STARTNEWWAVE_METHOD = "StartNewWave";

    void StartNewWave()
    {
        int amount = ((_wave) * 2) - 1;
        if (amount >= _maxZombies)
            amount = _maxZombies;
        SpawnManager.Instance.SpawnWave(amount);
    }

    private void Update()
    {
        _amountOfZombies = ZombieManager.Instance.GetZombies();
        _wave = SpawnManager.Instance.GetWave();

        if(!_initiatingWave)
            _initiatingWave = SpawnManager.Instance.GetInitiating();

        if (_amountOfZombies == 0 && !_initiatingWave)
        {
            _initiatingWave = true;
            //_currentFrequency = Mathf.Clamp(_currentFrequency - _waveFrequencyIncrement, _waveEndFrequency, _waveStartFrequency);

            Invoke(STARTNEWWAVE_METHOD, _waveDelay);
        }

        if(_amountOfZombies != 0)
        {
            _initiatingWave = false;
        }
    }
}
