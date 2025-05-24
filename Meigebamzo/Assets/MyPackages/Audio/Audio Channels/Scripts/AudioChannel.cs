using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AudioChannel")]
public class AudioChannel : ScriptableObject
{
    public int Value { get=>_value; set => _value = value; }
    public int ChannelNum => _channelNum;
    [SerializeField] int _channelNum;
    [SerializeField] int _value;

}