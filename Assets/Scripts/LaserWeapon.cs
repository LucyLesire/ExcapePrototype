using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class LaserWeapon : MonoBehaviour
{
    [SerializeField] private int _clipsSize = 100;
    [SerializeField] private List<Transform> _fireSockets = new List<Transform>();

    private bool _triggerPulled = false;
    private int _currentAmmo = 50;
    private float _hitTime = 0.0f;
    private const float _maxHitTime = 0.5f;
    private float _chargeTime = 0.0f;
    private const float _maxChargeTime = 1.5f;

    private LineRenderer _lineRenderer;
    private ParticleSystem _particleSystem;

    [SerializeField] private Image _ammoBar = null;
    [SerializeField] private Text _ammoCounter = null;

    [SerializeField] private AudioSource _laserChargeAudio = null;
    [SerializeField] private AudioSource _laserAudio = null;

    private Transform _rayTrans;
    public Transform RayTrans
    {
        get { return _rayTrans; }
        set { _rayTrans = value; }
    }

    public int CurrentAmmo
    {
        get
        {
            return _currentAmmo;
        }
    }

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();

    }

    private void Awake()
    {
        _currentAmmo = _clipsSize/2;
    }

    private void Update()
    {
        if (_triggerPulled)
        {
            FireLaser();
        }
        else
        {
            RemoveLaser();
            _chargeTime = 0.0f;
        }



        float ammoPercentage = ((float)_currentAmmo / _clipsSize);
        _ammoBar.transform.localScale = new Vector3(Mathf.Clamp(ammoPercentage, 0.0f, 1.0f), 1.0f, 1.0f);
        _ammoCounter.text = _currentAmmo.ToString();
    }

    private void FireLaser()
    {
        if (_currentAmmo <= 0)
        {
            RemoveLaser();
            return;
        }

        _chargeTime += Time.deltaTime;
        if(_chargeTime <= _maxChargeTime)
        {
            if(_particleSystem)
            {
                if(!_particleSystem.isPlaying)
                    _particleSystem.Play();
            }
            if (_laserChargeAudio)
            {
                if (!_laserChargeAudio.isPlaying)
                    _laserChargeAudio.Play();
            }
            _lineRenderer.SetPosition(1, _fireSockets[0].position);
            _lineRenderer.SetPosition(0, _fireSockets[0].position);
            return;
        }

        _hitTime += Time.deltaTime;
        if(_hitTime >= _maxHitTime)
        {
            --_currentAmmo;
            _hitTime -= _maxHitTime;
        }


        if (_laserChargeAudio)
        {
            if (_laserChargeAudio.isPlaying)
                _laserChargeAudio.Stop();
        }

        if(_laserAudio)
        {
            if (!_laserAudio.isPlaying)
                _laserAudio.Play();
        }

        _lineRenderer.SetPosition(0, _fireSockets[0].position);
        int layerMaskLimb = 11;
        RaycastHit hit;
        RaycastHit hitLimb;

        if (Physics.Raycast(_rayTrans.position, _rayTrans.TransformDirection(Vector3.forward), out hitLimb, float.MaxValue, layerMaskLimb))
        {
            _lineRenderer.SetPosition(1, hitLimb.point);
            Limb tmpLimb = hitLimb.collider.GetComponentInParent<Limb>();
            if(tmpLimb)
            {
                tmpLimb.BeingHit();
            }
        }
        else
        {
            if (Physics.Raycast(_rayTrans.position, _rayTrans.TransformDirection(Vector3.forward), out hit, float.MaxValue))
            {
                if (hit.collider)
                {
                    _lineRenderer.SetPosition(1, hit.point);
                }
            }
            else
            {
                _lineRenderer.SetPosition(1, _rayTrans.forward * 5000);
            }
        }
    }

    private void RemoveLaser()
    {
        if(_particleSystem)
        {
            if (_particleSystem.isPlaying)
            {
                _particleSystem.Stop();
            }
        }

        if (_laserChargeAudio)
        {
            if (_laserChargeAudio.isPlaying)
                _laserChargeAudio.Stop();
        }

        if (_laserAudio)
        {
            if (_laserAudio.isPlaying)
                _laserAudio.Stop();
        }


        _lineRenderer.SetPosition(1, _fireSockets[0].position);
        _lineRenderer.SetPosition(0, _fireSockets[0].position);
    }

    public void Fire()
    {
        _triggerPulled = true;
    }

    public void StopFire()
    {
        _triggerPulled = false;
    }

    public void Reload()
    {
        _currentAmmo = _clipsSize;
    }

    public void AddAmmo()
    {
        _currentAmmo += 25;
        _currentAmmo = Mathf.Clamp(_currentAmmo, 0, _clipsSize);
    }
}
