using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject _player = null;

    private void Update()
    {
        if (_player == null)
            TriggerGameOver();
    }

    void TriggerGameOver()
    {
        GameMaster.Instance.GameOverLabel = "Game Over!";
        GameMaster.Instance.TotalZombies = ZombieManager.Instance.TotalZombies;
        GameMaster.Instance.Timer = false;
        GameMaster.Instance.TotalWaves = SpawnManager.Instance.TotalWaves;

        SceneManager.LoadScene("GameOver");
    }
}
