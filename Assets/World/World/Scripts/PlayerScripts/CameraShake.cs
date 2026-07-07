using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public float shakeTime;

    CinemachineBasicMultiChannelPerlin shake;




    public IEnumerator ShakeCamera(float amplitude, float frequency)
    {
        shake.m_AmplitudeGain = amplitude;
        shake.m_FrequencyGain = frequency;
        yield return new WaitForSeconds(shakeTime);
        shake.m_AmplitudeGain = 0;
        shake.m_FrequencyGain = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        shake = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        shake.m_AmplitudeGain = 0;
        shake.m_FrequencyGain = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
