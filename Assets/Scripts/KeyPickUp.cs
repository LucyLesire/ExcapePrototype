using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    Quaternion _constantRotation;
    const float _speed = 50;
    bool _inHand = false;
    public bool InHand
    {
        set { _inHand = value; }
    }

    const string PLAYER_TAG = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == PLAYER_TAG)
        {
            other.gameObject.GetComponent<PlayerCharacter>().AddKey();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(!_inHand)
        {
            Vector3 deltaTimeRotation = Vector3.zero;
            deltaTimeRotation.y = Time.deltaTime * _speed;
            _constantRotation.eulerAngles += deltaTimeRotation;
            transform.rotation = _constantRotation;
        }
    }
}
