using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject SpawnTemplate = null;

    [SerializeField]
    private bool _active;
    public bool Active
    {
        get { return _active; }
        set { _active = value; }
    }

    private void OnEnable()
    {
        SpawnManager.Instance.RegisterSpawnPoint(this);
    }

    private void OnDisable()
    {
        SpawnManager.Instance.UnRegisterSpawnPoint(this);
    }

    public GameObject Spawn()
    {
        var zombie = Instantiate(SpawnTemplate, transform.position, transform.rotation);
        ZombieManager.Instance.RegisterZombie(zombie);

        //zombie.GetComponent<NavMeshMovementBehaviour>().NavMeshTarget;

        return zombie;
    }
}
