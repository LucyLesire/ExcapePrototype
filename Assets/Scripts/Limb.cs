using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{

    private float _hitTime;
    private const float _maxHitTime = 0.75f;
    Material _buildingMaterial = null;
    private bool _isDestroyed;
    public bool IsDestroyed
    {
        get { return _isDestroyed; }
        set { _isDestroyed = value; }
    }

    private string _tag;
    public string Tag
    {
        get { return _tag; }
    }

    private void Awake()
    {
        _buildingMaterial = GetComponentInChildren<Renderer>().material;
        _tag = gameObject.tag;
    }

    public void BeingHit()
    {
        _hitTime += Time.deltaTime;

        float percent = Mathf.Clamp01(1.0f / 300.0f);
        _buildingMaterial.color = new Color(_buildingMaterial.color.r * (1 - percent), _buildingMaterial.color.g * (1 - percent), _buildingMaterial.color.b * (1 - percent), _buildingMaterial.color.a);

        if (_hitTime >= _maxHitTime)
        {
            _buildingMaterial.color = Color.red;
            _hitTime = 0;
            _isDestroyed = true;
        }
    }
}
