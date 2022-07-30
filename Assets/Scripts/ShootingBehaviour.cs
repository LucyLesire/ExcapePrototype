using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _primaryGunTemplate = null;
    [SerializeField] private GameObject _secondaryGunTemplate = null;
    [SerializeField] private GameObject _primarySocket = null;

    [SerializeField] private GameObject _secondarySocket = null;
    [SerializeField] private Transform _rayTrans = null;

    private LaserWeapon _laserGun = null;
    private AxeWeapon _axeWeapon = null;
    private BasicWeapon _zombieWeapon = null;

    private int _totalAmmo = 25;
    public int TotalAmmo
    {
        get { return _totalAmmo - _laserGun.CurrentAmmo; }
    }

    public int Damage
    {
        get { return _zombieWeapon.Damage; }
        set { _zombieWeapon.Damage = value; }
    }
    public int PrimaryWeaponAmmo
    {
        get
        {
            if (_laserGun)
                return _laserGun.CurrentAmmo;
            else
                return 0;
        }
    }

    public int SecondaryWeaponAmmo
    {
        get
        {
            if (_axeWeapon)
                return /*_secondaryGun.CurrentAmmo*/ 0;
            else
                return 0;
        }
    }

    private void Awake()
    {
        if(_primaryGunTemplate != null && _primarySocket != null)
        {
            var gunObject = Instantiate(_primaryGunTemplate, _primarySocket.transform, true);
            gunObject.transform.localPosition = Vector3.zero;
            gunObject.transform.localRotation = Quaternion.identity;
            gunObject.transform.localScale = Vector3.one;
            _laserGun = gunObject.GetComponent<LaserWeapon>();
            _laserGun.RayTrans = _rayTrans;
        }
        if (_secondaryGunTemplate != null && _secondarySocket != null)
        {
            var gunObject = Instantiate(_secondaryGunTemplate, _secondarySocket.transform, false);
            gunObject.transform.localPosition = Vector3.zero;
            gunObject.transform.localRotation = Quaternion.identity;
            gunObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            if(gunObject.tag == "Axe")
            {
                _axeWeapon = gunObject.GetComponent<AxeWeapon>();
                _axeWeapon.RayTrans = _rayTrans;
            }
            else if(gunObject.tag == "Enemy")
            {
                _zombieWeapon = gunObject.GetComponent<BasicWeapon>();
            }
        }
    }

    public void PrimaryFire()
    {
        if(_laserGun != null)
        {
            _laserGun.Fire();
        }
    }

    public void StopPrimaryFire()
    {
        if (_laserGun != null)
        {
            _laserGun.StopFire();
        }
    }

    public void SecondaryFire()
    {
        if(_axeWeapon != null)
        {
            _axeWeapon.Fire();
        }
        else if(_zombieWeapon != null)
        {
            _zombieWeapon.Fire();
        }
    }

    public void Reload()
    {
        if(_laserGun != null)
        {
            _laserGun.Reload();
        }
        if (_axeWeapon != null)
        {
            //_secondaryGun.Reload();
        }
    }

    public void AddBattery()
    {
        _laserGun.AddAmmo();
        _totalAmmo += 10;
    }
}
