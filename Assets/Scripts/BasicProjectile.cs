using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] private float _speed = 30.0f;
    [SerializeField] private float _lifeTime = 10.0f;
    [SerializeField] private int _maxDamage = 0;
    private int _damage = 0;
    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    private bool _attacked = false;

    private void Awake()
    {
        Invoke(KILL_METHODNAME, _lifeTime);
    }

    private void FixedUpdate()
    {
        if(!WallDetection())
        {
            transform.position += transform.forward * Time.deltaTime * _speed;
        }
        if (_damage == 0)
        {
            _damage = _maxDamage;
        }
    }

    static readonly string[] RAYCAST_MASK = { "StaticLevel", "DynamicLevel" };

    bool WallDetection()
    {
        Ray collisionRay = new Ray(transform.position, transform.forward);
        if(Physics.Raycast(collisionRay, Time.deltaTime * _speed, LayerMask.GetMask(RAYCAST_MASK)))
        {
            Kill();
            return true;
        }
        return false;
    }

    const string KILL_METHODNAME = "Kill";

    void Kill()
    {
        Destroy(gameObject);
    }

    const string FRIENDLY_TAG = "Friendly";
    const string ENEMY_TAG = "EnemyAttack";

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == ENEMY_TAG)
        //    return;

        if (other.tag == tag)
            return;

        if(_attacked)
        {
            Kill();
            return;
        }

        Health otherHealth = other.GetComponent<Health>();

        if(otherHealth != null)
        {
            otherHealth.Damage(_damage);
            _attacked = true;
            Kill();
        }
    }
}
