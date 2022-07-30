using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikazeCharacter : BasicCharacter
{
    private GameObject _playerTarget = null;
    [SerializeField] private float _attackRange = 2.0f;
    [SerializeField] GameObject _attackVFXTemplate = null;
    private bool _hasAttacked = false;

    private List<Limb> _limbs = new List<Limb>();

    private void Start()
    {
        //expensive
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        GetComponentsInChildren<Limb>(false, _limbs);
        if (player) _playerTarget = player.gameObject;
    }

    private void Update()
    {
        HandleMovement();
        HandleAttacking();
    }

    void HandleMovement()
    {
        if (_movementBehaviour == null)
            return;



        _movementBehaviour.Target = _playerTarget;

        _movementBehaviour.DesiredLookatPoint = _playerTarget.transform.position;
    }

    void HandleAttacking()
    {
        if (_hasAttacked) return;

        if (_shootingBehaviour == null) return;

        if (_playerTarget == null) return;

        //if we are in range of the player, fire weapon, use sqr magnitude when comparing => more efficient
        if(((transform.position - _playerTarget.transform.position)).sqrMagnitude < _attackRange * _attackRange)
        {
            _shootingBehaviour.PrimaryFire();
            _hasAttacked = true;

            if (_attackVFXTemplate)
                Instantiate(_attackVFXTemplate, transform.position, transform.rotation);
            //After fire destroy it self

            Invoke(KILL_METHODNAME, 0.2f);
        }
    }

    const string KILL_METHODNAME = "Kill";

    void Kill()
    {
        Destroy(gameObject);
    }
}
