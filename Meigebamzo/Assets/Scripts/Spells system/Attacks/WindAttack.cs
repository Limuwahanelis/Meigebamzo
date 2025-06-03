using System.Collections.Generic;
using UnityEngine;

public class WindAttack : ContinousAttack
{
    List<WindPush> _windPushes = new List<WindPush>();
    AudioEvent _airAttackAudioEvent;
    AudioSource _audioSource;
    List<Transform> _windPushTrans = new List<Transform>();
    Transform _winPishPosHolder;
    public WindAttack(List<WindPush> windPushes, AudioEvent airAttackAudioEvent, AudioSource audioSource, List<Transform> windPushTrans, Transform winPishPosHolder)
    {
        IsContinous = false;
        _windPushes = windPushes;
        _audioSource = audioSource;
        _windPushTrans = windPushTrans;
        _winPishPosHolder = winPishPosHolder;
        _airAttackAudioEvent = airAttackAudioEvent;
    }
    public override void StartAttack()
    {
        _airAttackAudioEvent.Play(_audioSource);
        int windForce = _spells.FindAll(x => x.BasicElement.Element == Elements.Element.WIND).Count - 1;

        _windPushes[windForce].transform.position = _windPushTrans[windForce].transform.position;
        _windPushes[windForce].transform.up = _winPishPosHolder.up;
        _windPushes[windForce].transform.Rotate(_windPushes[windForce].transform.forward, 45f);
        _windPushes[windForce].SetPushForce(windForce + 1);
        _windPushes[windForce].GetComponent<Animator>().SetInteger("PushType", windForce);
        _windPushes[windForce].GetComponent<Animator>().SetTrigger("Push");
        _windPushes[windForce].transform.SetParent(null);
        _spells.Clear();
    }
    public override void Attack()
    {
       
    }

    public override void EndAttack()
    {
        
    }


}
