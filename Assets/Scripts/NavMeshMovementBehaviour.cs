using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovementBehaviour : MovementBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private Vector3 _previousTargetPosition;

    protected override void Awake()
    {
        base.Awake();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _movementSpeed;

        _previousTargetPosition = transform.position;
    }

    const float MOVEMENT_EPSILON = .25f;
    const float MOVEMENT_STOP = 3.9f;

    protected override void HandleMovement()
    {
        if(_navMeshTarget == null)
        {
            _navMeshAgent.isStopped = true;
            return;
        }

        if (_small)
        {
            MakeSmaller();
        }

        if ((_navMeshTarget - _previousTargetPosition).sqrMagnitude > MOVEMENT_EPSILON)
        {
            if((_navMeshAgent.transform.position - _navMeshTarget).sqrMagnitude > MOVEMENT_STOP)
            {
                NavMeshPath path = new NavMeshPath();
                _navMeshAgent.CalculatePath(_navMeshTarget, path);
                if (path.status != NavMeshPathStatus.PathPartial || path.status != NavMeshPathStatus.PathInvalid)
                {
                    if(_navMeshAgent.isOnNavMesh)
                    {
                        _navMeshAgent.SetDestination(_navMeshTarget);
                        _navMeshAgent.speed = _movementSpeed;
                        //_navMeshAgent.isStopped = false;
                        _previousTargetPosition = _navMeshTarget;
                    }
                }
                else
                {
                    _navMeshAgent.SetDestination(_navMeshAgent.transform.position);
                }
            }
            else
            {
                _navMeshAgent.SetDestination(_navMeshAgent.transform.position);
            }
        }

        transform.LookAt(_desiredLookatPoint);
    }

    protected void MakeSmaller()
    {
        if(_navMeshAgent.baseOffset != -1.4f)
        {
            _navMeshAgent.baseOffset = -1.4f;
            _actuallySmall = true;
        }
    }
}
