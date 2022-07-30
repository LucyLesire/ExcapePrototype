using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
public class SpawnManager : MonoBehaviour
{
    #region SINGLETON
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null && !_applicationQuiting)
            {
                _instance = FindObjectOfType<SpawnManager>();
                if (_instance == null)
                {
                    GameObject newObject = new GameObject("Singleton_SpawnManager");
                    _instance = newObject.AddComponent<SpawnManager>();
                }
            }
            return _instance;
        }
    }

    private static bool _applicationQuiting = false;
    public void OnApplicationQuit()
    {
        _applicationQuiting = true;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
    private const float _spawnDelayTime = 1.0f;
    private int _wave = 1;
    private bool _initiating = false;
    private const float _maxSpawnRange = 50.0f;

    public int TotalWaves
    {
        get { return _wave; }
    }

    private GameObject _player = null;
    public GameObject Player
    {
        get { return _player; }
        set { _player = value; }
    }

    public void RegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        if (!_spawnPoints.Contains(spawnPoint))
            _spawnPoints.Add(spawnPoint);
    }
    public void UnRegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        _spawnPoints.Remove(spawnPoint);
    }

    void Update()
    {
        //Remove any objects that are null
        _spawnPoints.RemoveAll(s => s == null);

        //will remove the first null as long as it finds any
    }

    public void SpawnWave(int amount)
    {
        _initiating = true;
        for(int i = 0; i < amount; i++)
        {
            Invoke(SPAWN_METHOD, _spawnDelayTime);
        }
        _initiating = false;
        _wave++;
    }

    const string SPAWN_METHOD = "SpawnZombie";

    private void SpawnZombie()
    {
        List<SpawnPoint> closestSpawnPoints = CalculateClosestSpawnPoints();
        int randIndex = Random.Range(0, closestSpawnPoints.Count);
        closestSpawnPoints[randIndex].Spawn();
    }

    private List<SpawnPoint> CalculateClosestSpawnPoints()
    {
        List<SpawnPoint> closestSpawnPoints = new List<SpawnPoint>();
        float closestDistance = float.MaxValue;
        int indexClosestOne = 0;

        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if(_spawnPoints[i].Active)
            {
                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(_spawnPoints[i].transform.position, _player.transform.position, NavMesh.AllAreas, path);

                float totalDistance = 0.0f;

                if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
                {
                    for (int j = 1; j < path.corners.Length; ++j)
                    {
                        totalDistance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                    }
                }

                //float sqrDistance = (_spawnPoints[i].transform.position - _player.transform.position).sqrMagnitude;
                if (totalDistance <= _maxSpawnRange)
                {
                    closestSpawnPoints.Add(_spawnPoints[i]);
                }

                if (totalDistance <= closestDistance)
                {
                    indexClosestOne = i;
                }
            }
        }

        if(closestSpawnPoints.Count == 0)
        {
            if(indexClosestOne != 0)
                closestSpawnPoints.Add(_spawnPoints[indexClosestOne]);
        }

        return closestSpawnPoints;
    }

    public int GetWave()
    {
        return _wave;
    }

    public bool GetInitiating()
    {
        return _initiating;
    }

    public void ResetWave()
    {
        _wave = 1;
    }
}

