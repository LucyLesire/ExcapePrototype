using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] 
    private int _startHealth = 10;
    private int _currentHealth = 0;

    private int _totalHearts = 0;
    public int TotalHearts
    {
        get { return _totalHearts; }
    }

    [SerializeField] private Color _flickerColor = Color.white;
    [SerializeField] private float _flickerDuration = 0.1f;

    private Color _startColor;
    private Material _attachedMaterial;
    private Material _attachedMaterial2;

    private AudioSource _hurtAudio;

    const string _playerMatName = "MAT_Player (Instance)";
    const string COLOR_PARAMETER = "_Color";

    public float HealthValue
    {
        get
        {
            return _currentHealth/10;
        }
    }

    private void Awake()
    {
        _currentHealth = _startHealth;
        _hurtAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Renderer[] renderer = transform.GetComponentsInChildren<Renderer>();
        if (renderer.Length != 0)
        {
            for(int i = 0; i < renderer.Length; i++)
            {
                if(renderer[i].material.name == _playerMatName)
                {
                    if(!_attachedMaterial)
                    {
                        _attachedMaterial = renderer[i].material;
                    }
                    else if(!_attachedMaterial2)
                    {
                        _attachedMaterial2 = renderer[i].material;
                    }
                }
            }
            //this will behind the scenes create new instance of material

            if (_attachedMaterial)
                _startColor = _attachedMaterial.GetColor(COLOR_PARAMETER);
        }
    }

    public void Damage(int amount)
    {
        _currentHealth -= amount;


        if(_attachedMaterial)
        {
            _attachedMaterial.SetColor(COLOR_PARAMETER, _flickerColor);
            
            if(_attachedMaterial2)
                _attachedMaterial2.SetColor(COLOR_PARAMETER, _flickerColor);
            
            Invoke(RESET_COLOR_METHOD, _flickerDuration);
        }

        if(_hurtAudio)
        {
            if (!_hurtAudio.isPlaying)
                _hurtAudio.Play();
        }

        if(_currentHealth <= 0)
        {
            Kill();
        }
    }

    const string RESET_COLOR_METHOD = "ResetColor";

    void ResetColor()
    {
        if (!_attachedMaterial)
            return;
        _attachedMaterial.SetColor(COLOR_PARAMETER, _startColor);

        if (!_attachedMaterial2)
            return;
        _attachedMaterial2.SetColor(COLOR_PARAMETER, _startColor);
    }

    void Kill()
    {

        GameMaster.Instance.TotalHearts = _totalHearts;
        GameMaster.Instance.TotalAmmo = gameObject.GetComponent<ShootingBehaviour>().TotalAmmo;

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_attachedMaterial != null)
            return;
        Destroy(_attachedMaterial);

        if (_attachedMaterial2 != null)
            return;
        Destroy(_attachedMaterial2);
    }

    public void AddHealth()
    {
        _currentHealth += 20;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _startHealth);
        _totalHearts++;
    }

}
