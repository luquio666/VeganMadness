using Spine;
using Spine.Unity;
using System.Collections;
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
    public Animator SkelAnim;
    public Transform SkeletonParent;
    public float KickCooldown = .5f;
    public float KickAnim = .5f;
    public float KickFxSpeed = 3f;
    public float KickDestroyTime = 1f;
    float _cooldown;
    bool _gameStarted;
    bool _lastAnimWasKick;

    public void StartGame()
    {
        _gameStarted = true;
        _cooldown = KickCooldown;
        SkelAnim.Play("idle_front", 0, 0f);
    }

    public void EndGame()
    {
        _gameStarted = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "badFood")
        {
            SkelAnim.Play("hit_front", 0, 0f);
            GameEvents.PlayerGetsHit();
        }
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
        switch (id)
        {
            case Direction.UP:
                movement = Vector3.forward;
                break;
            case Direction.RIGHT:
                movement = Vector3.right;
                SkeletonParent.localScale = new Vector3(-1, 1, 1);
                break;
            case Direction.DOWN:
                movement = Vector3.back;
                break;
            case Direction.LEFT:
                movement = Vector3.left;
                SkeletonParent.localScale = new Vector3(1, 1, 1);
                break;
            default:
                break;
        }

        var kickFX = Instantiate(KickSmoke, (movement/2), Quaternion.identity);
        kickFX.GetComponent<KickFx>().ConfigKickFx(movement, KickFxSpeed, KickDestroyTime);

        if(Random.Range(0,2) > 0)
            SkelAnim.Play("punch_front", 0, 0f);
        else
            SkelAnim.Play("kick_front", 0, 0f);

        yield return new WaitForSeconds(KickAnim);

        yield return null;
    }

}
