using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
[RequireComponent(typeof(NavMeshObstacle))]

public class Gate : MonoBehaviour
{
    [SerializeField] GameObject _fence = null;
    [SerializeField] BoxCollider _triggerCollider = null;
    [SerializeField] GameObject[] _spawnPointsNext = null;
    [SerializeField] int _waveToOpen = 0;

    private bool _isOpen = false;

    NavMeshObstacle _navMeshObstacle = null;

    private void Awake()
    {
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    public void OpenGate()
    {
        if(_spawnPointsNext.Length !=0)
        {
            foreach (GameObject s in _spawnPointsNext)
            {
                s.GetComponent<SpawnPoint>().Active = true;
            }
        }
        Destroy(_fence);
        _navMeshObstacle.enabled = false;
        _triggerCollider.enabled = false;
    }

    public void Update()
    {
        if(!_isOpen)
        {
            int wave = SpawnManager.Instance.GetWave();
            if (wave >= _waveToOpen)
            {
                OpenGate();
                _isOpen = true;
            }
        }

    }
}
