using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] Text _timeCounter = null;
    [SerializeField] Text _zombieCounter = null;
    [SerializeField] Text _heartsCounter = null;
    [SerializeField] Text _ammoCounter = null;
    [SerializeField] Text _waveCounter = null;
    [SerializeField] Text _gameOverLabel = null;

    private void Start()
    {
        float totalTime = GameMaster.Instance.TotalTime;
        string timerString = ((int)totalTime).ToString();
        timerString += "s";
        _timeCounter.text = timerString;
        _zombieCounter.text = GameMaster.Instance.TotalZombies.ToString();
        _heartsCounter.text = GameMaster.Instance.TotalHearts.ToString();

        int ammo = GameMaster.Instance.TotalAmmo;
        if(ammo <= 0)
        {
            ammo = 0;
        }
        _ammoCounter.text = ammo.ToString();
        _waveCounter.text = (GameMaster.Instance.TotalWaves).ToString();

        _gameOverLabel.text = GameMaster.Instance.GameOverLabel;

        Cursor.visible = true;
    }
}
