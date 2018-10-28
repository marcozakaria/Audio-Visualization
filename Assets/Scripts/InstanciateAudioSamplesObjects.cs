using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciateAudioSamplesObjects : MonoBehaviour
{
    public GameObject sampleCubePrefap;

    public float MaxYscale;  // scale upwards
    public int flatScale;  // scale to make a circle

    GameObject[] sampleCubes = new GameObject[512];

    const float rotationDegree = 0.703125f; // equal(360/512)

	void Start ()
    {
        for (int i = 0; i < 512; i++)
        {
            GameObject instance = (GameObject)Instantiate(sampleCubePrefap);
            instance.transform.position = this.transform.position + (Vector3.forward*flatScale);
            instance.transform.parent = this.transform;
            instance.name = "SampleCube" + i;
            this.transform.eulerAngles = new Vector3(0, -rotationDegree * i, 0); // rotate every cub to make a good circle shape
            //instance.transform.position = Vector3.forward * flatScale;
            sampleCubes[i] = instance;
        }
	}

	void Update ()
    {
        for (int i = 0; i < 512; i++)
        {
            if (sampleCubes != null)
            {   // scale them as they get info from the song
                sampleCubes[i].transform.localScale = new Vector3(10, AudioPeer.audioSamples[i] * MaxYscale, 10);
            }
        }
	}
}
