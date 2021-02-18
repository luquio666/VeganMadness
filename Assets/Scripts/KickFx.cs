using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickFx : MonoBehaviour
{
    Vector3 _direction;
    float _speed;

    public void ConfigKickFx(Vector3 direction, float speed, float destroyAfter)
    {
        _direction = direction;
        _speed = speed;
        Invoke("AutoDestroy", destroyAfter);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "badFood")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log(Time.time + "_" + other.tag);
        }
    }

    void Update()
    {
        transform.Translate(_direction * Time.deltaTime * _speed);
    }

        void AutoDestroy()
    {
        Destroy(this.gameObject);
    }
}
