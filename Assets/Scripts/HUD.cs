using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Text _waveCounter = null;
    [SerializeField] Text _primaryAmmo = null;

    [SerializeField] Sprite _fullHeart = null;
    [SerializeField] Sprite _halfHeart = null;
    [SerializeField] Sprite _emptyHeart = null;

    [SerializeField] List<Image> _hearts = new List<Image>();

    private Health _playerHealth;
    private ShootingBehaviour _playerShootingBehaviour = null;

    private void Start()
    {
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();

        if(player != null)
        {
            _playerHealth = player.GetComponent<Health>();
            _playerShootingBehaviour = player.GetComponent<ShootingBehaviour>();
        }
    }

    private void Update()
    {
        SyncData();
    }

    void SyncData()
    {
        //health
        if(_playerHealth)
        {
            int playerHealth = ((int)(_playerHealth.HealthValue));
            for(int i = 0; i < _hearts.Count; i++)
            {
                if(i < playerHealth / 2)
                {
                    _hearts[i].sprite = _fullHeart;
                }
                else
                {
                    _hearts[i].sprite = _emptyHeart;
                }
            }

            if (playerHealth % 2 == 1)
            {
                _hearts[playerHealth/2].sprite = _halfHeart;
            }
        }

        //ammo
        if(_primaryAmmo && _playerShootingBehaviour)
        {
            _primaryAmmo.text = _playerShootingBehaviour.PrimaryWeaponAmmo.ToString();
        }

        if (_waveCounter)
        {
            _waveCounter.text = (SpawnManager.Instance.GetWave() - 1).ToString();
        }
    }
}
