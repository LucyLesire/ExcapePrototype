using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Vector3 _previousPosition;
    private Animator _animator = null;

    private void Awake()
    {
        _previousPosition = transform.root.position;

        _animator = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovementAnimation();
    }

    const string IS_MOVING_PARAMETER = "isMoving";

    void HandleMovementAnimation()
    {
        if(_animator == null)
        {
            return;
        }

        _animator.SetBool(IS_MOVING_PARAMETER, (transform.root.position - _previousPosition).sqrMagnitude > 0.0001f);

        _previousPosition = transform.root.position;
    }
}
