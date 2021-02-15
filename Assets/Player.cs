﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    NONE,
    UP,
    RIGHT,
    DOWN,
    LEFT
}

public class Player : MonoBehaviour
{
    public GameObject KickSmoke;
    public GameObject Foot;
    public float KickCooldown = .5f;
    public float KickAnim = .5f;
    public float KickFxSpeed = 3f;
    public float KickDestroyTime = 1f;
    float _cooldown;
    bool _gameStarted;

    public void StartGame()
    {
        _gameStarted = true;
        _cooldown = KickCooldown;
    }

    public void EndGame()
    {
        _gameStarted = false;
    }

    void Update()
    {
        if (_gameStarted == false)
            return;

        if (_cooldown > 0)
            _cooldown -= Time.deltaTime;

        Inputs();
    }

    void Inputs()
    {
        if (_cooldown > 0)
            return;

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            TriggerAnim(Direction.UP);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            TriggerAnim(Direction.RIGHT);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            TriggerAnim(Direction.DOWN);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            TriggerAnim(Direction.LEFT);
        }
    }


    Coroutine _triggerAnimCo;
    void TriggerAnim(Direction id)
    {
        if (_triggerAnimCo != null)
            StopCoroutine(_triggerAnimCo);
        _triggerAnimCo = StartCoroutine(TriggerAnimCo(id));
    }

    Vector3 movement;
    IEnumerator TriggerAnimCo(Direction id)
    {
        _cooldown = KickCooldown;
        Foot.SetActive(false);
        switch (id)
        {
            case Direction.NONE:
                Foot.SetActive(false);
                break;
            case Direction.UP:
                movement = Vector3.forward;
                break;
            case Direction.RIGHT:
                movement = Vector3.right;
                break;
            case Direction.DOWN:
                movement = Vector3.back;
                break;
            case Direction.LEFT:
                movement = Vector3.left;
                break;
            default:
                break;
        }
        transform.rotation = Quaternion.LookRotation(movement);

        yield return new WaitForSeconds(KickAnim/4f);
        Foot.SetActive(true);

        var kickFX = Instantiate(KickSmoke, (movement/2)+Vector3.up, Quaternion.identity);
        kickFX.GetComponent<KickFx>().ConfigKickFx(movement, KickFxSpeed, KickDestroyTime);

        yield return new WaitForSeconds(KickAnim);
        Foot.SetActive(false);

        yield return null;
    }

}
