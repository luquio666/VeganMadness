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
    public SkeletonAnimation SkelAnim;
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
        SkelAnim.state.SetAnimation(0, "idle_front", false).TrackEnd = float.PositiveInfinity;
    }

    public void EndGame()
    {
        _gameStarted = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "badFood")
        {
            SkelAnim.state.SetAnimation(0, "hit_front", false);
            SkelAnim.state.AddAnimation(0, "idle_front", true, 0.1667f);
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

        yield return new WaitForSeconds(KickAnim/4f);

        var kickFX = Instantiate(KickSmoke, (movement/2), Quaternion.identity);
        kickFX.GetComponent<KickFx>().ConfigKickFx(movement, KickFxSpeed, KickDestroyTime);

        if(Random.Range(0,2) > 0)
            SkelAnim.state.SetAnimation(0, "punch_front", false);
        else
            SkelAnim.state.SetAnimation(0, "kick_front", false);

        SkelAnim.state.AddAnimation(0, "idle_front", true, 0.1667f);

        yield return new WaitForSeconds(KickAnim);

        yield return null;
    }

}
