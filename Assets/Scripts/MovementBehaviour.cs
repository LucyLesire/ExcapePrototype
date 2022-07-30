using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovementBehaviour : MonoBehaviour
{
    [SerializeField]
    protected float _movementSpeed = 1.0f;
    public float MovementSpeed
    {
        get { return _movementSpeed; }
        set { _movementSpeed = value; }
    }
    protected Rigidbody _rigidBody;

    protected Vector3 _desiredMovementDirection = Vector3.zero;
    public Vector3 DesiredMovementDirection
    {
        get { return _desiredMovementDirection; }
        set { _desiredMovementDirection = value; }
    }

    protected Vector3 _desiredLookatPoint = Vector3.zero;
    public Vector3 DesiredLookatPoint
    {
        get { return _desiredLookatPoint; }
        set { _desiredLookatPoint = value; }
    }

    protected GameObject _target;
    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

    protected Vector3 _navMeshTarget;
    public Vector3 NavMeshTarget
    {
        get { return _navMeshTarget; }
        set { _navMeshTarget = value; }
    }

    protected bool _small = false;
    public bool Small
    {
        get { return _small; }
        set { _small = value; }
    }

    protected bool _actuallySmall = false;
    public bool ActuallySmall
    {
        get { return _actuallySmall; }
    }

    // horizontal rotation speed
    public float horizontalSpeed = 1f;
    // vertical rotation speed
    public float verticalSpeed = 1f;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    private Camera cam = null;

    protected virtual void Start()
    {
        if(gameObject.tag == "Player")
            cam = Camera.main;
    }

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        HandleRotation();
    }
    protected virtual void FixedUpdate()
    {
        HandleMovement();
    }

    protected virtual void HandleMovement()
    {
        Vector3 movement = _desiredMovementDirection.normalized;
        movement *= _movementSpeed;

        _rigidBody.velocity = movement;
    }

    protected virtual void HandleRotation()
    {
        if (cam == null)
            return;

        float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cam.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
        transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
    }
}

