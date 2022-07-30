using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZombieCharacter : BasicCharacter
{
    private GameObject _playerTarget = null;
    [SerializeField] private float _attackRange = 2.0f;
    [SerializeField] GameObject _attackVFXTemplate = null;
    private bool _hasAttacked = false;

    [SerializeField] List<GameObject> _drops = null;

    private float _knockBackTime = 1.0f;
    private const string CANCELKNOCKBACK_METHOD = "CancelKnockback";

    private const string ANIM_ISATTACKING = "isAttacking";

    protected bool _lostALeg = false;
    protected bool _lostAArm = false;
    protected bool _noArms = false;
    private List<Limb> _limbs = new List<Limb>();

    private Animator _zombieAnim = null;

    private float _attackDelay = 0.0f;
    private const float _maxAttackDelay = 1.5f;

    private bool _knockedBack = false;
    public bool KnockedBack
    {
        set { _knockedBack = value; }
    }

    private void Start()
    {
        //expensive
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        GetComponentsInChildren<Limb>(false, _limbs);
        if (player) _playerTarget = player.gameObject;

        _zombieAnim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleAttacking();
        CheckLimbs();
    }

    void HandleMovement()
    {
        if (_movementBehaviour == null)
            return;


        if(!_knockedBack)
        {
            _movementBehaviour.NavMeshTarget = _playerTarget.transform.position;
        }
        else
        {
            _movementBehaviour.NavMeshTarget = (gameObject.transform.position - gameObject.transform.forward * 3.0f);
            Invoke(CANCELKNOCKBACK_METHOD, _knockBackTime);
        }
        _movementBehaviour.DesiredLookatPoint = _playerTarget.transform.position;
    }

    private void CancelKnockback()
    {
        _knockedBack = false;
    }

    private void CheckLimbs()
    {
        if(_limbs.Count == 1)
        {
            int random = Random.Range(0, _drops.Count * 6);
            if(random < _drops.Count * 3)
            {
                GameObject randomDrop;
                if(random < 4)
                {
                    randomDrop = _drops[0];
                }
                else if(random < 8)
                {
                    randomDrop = _drops[1];
                }
                else
                {
                    randomDrop = _drops[2];
                }

                Vector3 posDrop = gameObject.transform.position;
                if(_movementBehaviour.ActuallySmall)
                {
                    posDrop.y += 0.6f;
                }
                Instantiate(randomDrop, posDrop, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        else
        {
            for (int i = 0; i < _limbs.Count; i++)
            {
                if (_limbs[i].IsDestroyed)
                {
                    if (_limbs[i].tag == "Leg")
                    {
                        if (_lostALeg)
                        {
                            _movementBehaviour.MovementSpeed /= 2.0f;
                            _movementBehaviour.Small = true;
                        }
                        else
                        {
                            _lostALeg = true;
                            _movementBehaviour.MovementSpeed /= 1.5f;
                        }
                    }
                    else if (_limbs[i].tag == "Arm")
                    {
                        if (_lostAArm)
                        {
                            _noArms = true;
                        }
                        else
                        {
                            _lostAArm = true;
                            _shootingBehaviour.Damage /= 2;
                        }
                    }
                    Destroy(_limbs[i].gameObject);
                    _limbs.RemoveAt(i);
                }
            }
        }
       
    }

    void HandleAttacking()
    {

        if (_shootingBehaviour == null) return;

        if (_playerTarget == null) return;

        if(!_noArms)
        {
            if (_hasAttacked)
            {
                _attackDelay += Time.deltaTime;
                if (_attackDelay >= _maxAttackDelay)
                {
                    _attackDelay = 0.0f;
                    _hasAttacked = false;

                }
            }
            //if we are in range of the player, fire weapon, use sqr magnitude when comparing => more efficient
            else if (((transform.position - _playerTarget.transform.position)).sqrMagnitude < _attackRange * _attackRange)
            {
                if (_zombieAnim)
                {
                    _zombieAnim.SetBool(ANIM_ISATTACKING, true);

                    Invoke(FIRE_METHOD, 0.75f);
                    _hasAttacked = true;

                    if (_attackVFXTemplate)
                        Instantiate(_attackVFXTemplate, transform.position, transform.rotation);
                }
            }
        }


    }

    const string FIRE_METHOD = "Fire";
    void Fire()
    {
        _shootingBehaviour.SecondaryFire();
        _zombieAnim.SetBool(ANIM_ISATTACKING, false);
    }
}
