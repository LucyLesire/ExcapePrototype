using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : MonoBehaviour
{
    [SerializeField] private GameObject _bulletTemplate = null;
    [SerializeField] private int _clipsSize = 50;
    [SerializeField] private float _fireRate = 25.0f;
    [SerializeField] private List<Transform> _fireSockets = new List<Transform>();

    private bool _triggerPulled = false;
    private int _currentAmmo = 50;
    private float _fireTimer = 0.0f;
    [SerializeField] private int _damage = 0;
    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    //[SerializeField] private AudioSource _fireSound;

    public int CurrentAmmo
    {
        get
        {
            return _currentAmmo;
        }
    }

    private void Awake()
    {
        _currentAmmo = _clipsSize;
    }
    private void Update()
    {
        if(_fireTimer > 0.0f)
        {
            _fireTimer -= Time.deltaTime;
        }

        if(_fireTimer <= 0.0f && _triggerPulled)
        {
            FireProjectile();
        }

        _triggerPulled = false;
    }

    private void FireProjectile()
    {
        if (_currentAmmo <= 0)
            return;

        if (_bulletTemplate == null)
            return;

        --_currentAmmo;

        for(int i = 0; i < _fireSockets.Count; i++)
        {
            var bullet = Instantiate(_bulletTemplate, _fireSockets[i].position, _fireSockets[i].rotation);
            bullet.gameObject.GetComponent<BasicProjectile>().Damage = _damage;
        }

        _fireTimer += 1.0f / _fireRate;

        //if (_fireSound)
        //    _fireSound.Play();
    }

    public void Fire()
    {
        _triggerPulled = true;
    }

    public void Reload()
    {
        _currentAmmo = _clipsSize;
    }



}
