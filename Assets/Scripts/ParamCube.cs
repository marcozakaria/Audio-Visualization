using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public int band;
    public float startScale, scaleMultiplier;
    public bool useBuffer;

    private void Update()
    {
        if (!useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (AudioPeer.frequencyBand[band] * scaleMultiplier) + startScale, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, (AudioPeer.bandBuffer[band] * scaleMultiplier) + startScale, transform.localScale.z);
        }
    }

}
