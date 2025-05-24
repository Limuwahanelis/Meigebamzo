using UnityEngine;
using UnityEngine.UI;

public class AudioSliderTest : MonoBehaviour
{
    [SerializeField] AudioChannel _audioChannel;

    [SerializeField] Slider _slider;

    public void OnSliderChangevalue(float a)
    {
        //AudioTest.AudioChannels[_audioChannel.ChannelNum].Value= (int)a;

    }
}
