using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Direction Direction;

    public GameObject SpawnAlert;

    Vector3 _direction;

    private void Start()
    {
        switch (Direction)
        {
            case Direction.NONE:
                break;
            case Direction.UP:
                _direction = Vector3.forward;
                break;
            case Direction.RIGHT:
                _direction = Vector3.right;
                break;
            case Direction.DOWN:
                _direction = Vector3.back;
                break;
            case Direction.LEFT:
                _direction = Vector3.left;
                break;
            default:
                break;
        }
    }

    public void SpawnItem(GameObject item, float speed)
    {
        Vector3 relativePos = this.transform.position - _direction;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);

        var itemGo = Instantiate(item, this.transform.position, rotation);
        itemGo.GetComponent<Item>().ConfigItem(Vector3.back, speed);

        if (SpawnAlert != null)
        {
            if (_spawnAlertCo != null)
            {
                StopCoroutine(_spawnAlertCo);
                SpawnAlert.SetActive(false);
            }
            _spawnAlertCo = StartCoroutine(SpawnAlertCo());
        }
    }

    Coroutine _spawnAlertCo;
    IEnumerator SpawnAlertCo()
    {
        SpawnAlert.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        SpawnAlert.SetActive(false);
    }
}
