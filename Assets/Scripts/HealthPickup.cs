using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    Quaternion _constantRotation;
    const float _speed = 50;

    const string PLAYER_TAG = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == PLAYER_TAG)
        {
            other.gameObject.GetComponent<Health>().AddHealth();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Vector3 deltaTimeRotation = Vector3.zero;
        deltaTimeRotation.y = Time.deltaTime * _speed;
        _constantRotation.eulerAngles += deltaTimeRotation;
        transform.rotation = _constantRotation;
    }
}
