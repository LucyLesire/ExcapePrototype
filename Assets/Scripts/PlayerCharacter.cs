using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BasicCharacter
{

    const string MOVEMENT_HORIZONTAL = "MovementHorizontal";
    const string MOVEMENT_VERTICAL = "MovementVertical";
    const string GROUND_LAYER = "Ground";
    const string PRIMARY_FIRE = "PrimaryFire";

    const string SECONDARY_FIRE = "SecondaryFire";
    const string USE_ITEM = "UseItem";
    const string RELOAD = "Reload";

    private Plane _cursorMovementPlane;
    private float _maxHitRange = 10.0f;

    [SerializeField] GameObject _keyTemplate = null;
    [SerializeField] Transform _keySocket = null;

    GameObject _keyObject;

    bool _hasKey = false;

    protected override void Awake()
    {
        base.Awake();
        _cursorMovementPlane = new Plane(Vector3.up, transform.position);
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        HandleFireInput();
        HandleKeyInput();
        Vector3 direction = _keySocket.TransformDirection(Vector3.forward);

        Debug.DrawRay(_keySocket.position, direction * 10, Color.red, 0.2f);
    }

    private void FixedUpdate()
    {
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        if (_movementBehaviour == null)
            return;

        //movement
        float horizontalMovement = Input.GetAxis(MOVEMENT_HORIZONTAL);
        float verticalMovement = Input.GetAxis(MOVEMENT_VERTICAL);

        Vector3 movement = transform.right * horizontalMovement + transform.forward * verticalMovement;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            movement.y = 1000.0f;
        }
        else
        {
            movement.y = 0.0f;
        }

        _movementBehaviour.DesiredMovementDirection = movement;
    }

    void HandleFireInput()
    {
        if (_shootingBehaviour == null)
            return;

        if (Input.GetAxis(PRIMARY_FIRE) > 0.0f)
        {
            _shootingBehaviour.PrimaryFire();
        }
        if(Input.GetAxis(PRIMARY_FIRE) <= 0.0f)
        {
            _shootingBehaviour.StopPrimaryFire();
        }
        if (Input.GetAxis(SECONDARY_FIRE) > 0.0f)
        {
            _shootingBehaviour.SecondaryFire();
        }
        if (Input.GetAxis(RELOAD) > 0.0f)
        {
            _shootingBehaviour.Reload();
        }
    }

    void HandleKeyInput()
    {
        if (Input.GetAxis(USE_ITEM) > 0.0f)
        {
            if (_hasKey)
            {
                int layerMaskGate = LayerMask.GetMask("Gate");
                RaycastHit hit;
                Vector3 direction = _keySocket.TransformDirection(Vector3.forward);
                Vector3 pos = _keySocket.position;
                pos.x -= 2.0f;

                if (Physics.Raycast(pos, direction, out hit, _maxHitRange, layerMaskGate))
                {
                    hit.collider.gameObject.GetComponent<Gate>().OpenGate();
                    Destroy(_keyObject);
                    _hasKey = false;
                }
            }
        }
    }

    public void AddKey()
    {
        if (!_hasKey)
        {
            _keyObject = Instantiate(_keyTemplate, _keySocket.transform, false);
            _keyObject.transform.localPosition = Vector3.zero;
            _keyObject.transform.localScale = Vector3.one;
            _keyObject.GetComponent<KeyPickUp>().InHand = true;

            _hasKey = true;
        }

    }
}

