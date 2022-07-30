using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeWeapon : MonoBehaviour
{
    private bool _triggerPulled = false;
    private bool _canShoot = true;
    [SerializeField] private float _maxHitRange = 0.5f;

    private Animator _axeAnim = null;
    private Limb _hitLimb;
    private EnemyZombieCharacter _hitZombie;

    private AudioSource _axeAudio;


    private Transform _rayTrans;
    public Transform RayTrans
    {
        get { return _rayTrans; }
        set { _rayTrans = value; }
    }

    private const string AXE_ATTACK = "isAttacking";
    private const string IDLE_ANIMATION = "IdleAxe";
    private const string ATTACK_ANIMATION = "AttackAxe";

    private void Awake()
    {
        _axeAnim = gameObject.GetComponent<Animator>();
        _axeAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_axeAnim.GetCurrentAnimatorStateInfo(0).IsName(ATTACK_ANIMATION) && (_axeAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= _axeAnim.GetCurrentAnimatorStateInfo(0).length))
        {
            _canShoot = true;
            _axeAnim.SetBool(AXE_ATTACK, false);
            if (_axeAudio)
                _axeAudio.Stop();
            DestroyLimb();
        }
        else if (_triggerPulled && _canShoot)
        {
            _axeAnim.SetBool(AXE_ATTACK, true);
            FireAxe();
            if (_axeAudio)
                _axeAudio.Play();
        }

        _triggerPulled = false;

    }

    private void FireAxe()
    {
        _canShoot = false;
        int layerMaskLimb = 11;
        RaycastHit hitLimb;
        Vector3 direction = _rayTrans.transform.TransformDirection(Vector3.forward);
        //direction.y = 0.0f;

        if (Physics.Raycast(_rayTrans.position, direction, out hitLimb, _maxHitRange, layerMaskLimb))
        {
            _hitLimb = hitLimb.collider.gameObject.GetComponentInParent<Limb>();
            _hitZombie = hitLimb.collider.gameObject.GetComponent<EnemyZombieCharacter>();
        }
    }

    private void DestroyLimb()
    {
        if(_hitLimb)
        {
            _hitLimb.IsDestroyed = true;
        }
        if(_hitZombie)
        {
            _hitZombie.KnockedBack = true;
        }
    }

    public void Fire()
    {
        if(_canShoot)
        {
            _triggerPulled = true;
        }
    }
}
