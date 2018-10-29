using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnAudio : MonoBehaviour
{
    public int band;
    public float minIntensty, maxIntinisty;
    Light light;

    private void Start()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        light.intensity = (AudioPeer.audioBandBuffer[band] * (maxIntinisty - minIntensty)) + minIntensty;
    }

}
