using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Vector3 _direction;
    float _speed;
    bool _enableMove;

    public void ConfigItem(Vector3 direction, float speed)
    {
        _direction = direction;
        _speed = speed;
        _enableMove = true;
    }

    public void EnableMove()
    {
        _enableMove = true;
    }
    public void DisableMove()
    {
        _enableMove = false;
    }

    void Update()
    {
        if (_enableMove == false)
            return;

        if (Vector3.Distance(this.transform.position, Vector3.zero) < 1f)
            Destroy(this.gameObject);

        transform.Translate(_direction * Time.deltaTime * _speed);
    }

}
