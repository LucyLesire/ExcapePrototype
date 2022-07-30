using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            SceneManager.LoadScene("Menu");
            return;
        }

        if(other.tag == "Player")
        {
            GameMaster.Instance.TotalZombies = ZombieManager.Instance.TotalZombies;
            GameMaster.Instance.TotalHearts = other.gameObject.GetComponent<Health>().TotalHearts;
            GameMaster.Instance.TotalAmmo = other.gameObject.GetComponent<ShootingBehaviour>().TotalAmmo;
            GameMaster.Instance.Timer = false;
            GameMaster.Instance.TotalWaves = SpawnManager.Instance.TotalWaves;

            GameMaster.Instance.GameOverLabel = "You escaped!";

            SceneManager.LoadScene("GameOver");
        }
    }
}
