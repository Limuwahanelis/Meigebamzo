using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AudioVolumes:MonoBehaviour
{
    public static List<AudioChannel> AudioChannels => _audioChannels;

    [SerializeField] List<AudioChannel> _audioChannelsReferences = new List<AudioChannel>();

    private static List<AudioChannel> _audioChannels;


    private void Awake()
    {
        _audioChannels = _audioChannelsReferences;
        _audioChannels.Sort((c1, c2) => c1.ChannelNum.CompareTo(c2.ChannelNum));
    }
}
