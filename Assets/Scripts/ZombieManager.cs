using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    #region SINGLETON
    private static ZombieManager _instance;
    public static ZombieManager Instance
    {
        get
        {
            if (_instance == null && !_applicationQuiting)
            {
                _instance = FindObjectOfType<ZombieManager>();
                if (_instance == null)
                {
                    GameObject newObject = new GameObject("Singleton_ZombieManager");
                    _instance = newObject.AddComponent<ZombieManager>();
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

    private List<GameObject> _zombies = new List<GameObject>();
    private int _totalZombies;
    public int TotalZombies
    {
        get { return _totalZombies - _zombies.Count; }
    }

    public void RegisterZombie(GameObject zombie)
    {
        if (!_zombies.Contains(zombie))
        {
            _zombies.Add(zombie);
            _totalZombies++;
        }
    }
    public void UnRegisterZombie(GameObject zombie)
    {
        _zombies.Remove(zombie);
    }

    void Update()
    {
        //Remove any objects that are null
        _zombies.RemoveAll(z => z == null);

        //will remove the first null as long as it finds any
    }

    public int GetZombies()
    {
        return _zombies.Count;
    }
}

