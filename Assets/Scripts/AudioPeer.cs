using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    public static float[] audioSamples = new float[512];
    public static float[] frequencyBand = new float[8];
    public static float[] bandBuffer = new float[8];  // frequency higher than band , band bufffer become the frequency buffer
    float[] bufferDecrease = new float[8];

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
    }

    void BandBuffer() // smoth transition to give it better look 
    {
        for (int i = 0; i < 8; i++)
        {
            if (frequencyBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = frequencyBand[i];
                bufferDecrease[i] = 0.005f;
            }
            if (frequencyBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands() // split into 8 frequency segments
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0; // average for each segment of bands
            int sampleCount = (int)Mathf.Pow(2, i) * 2;  // get number of samples or each band

            if (i==7) // last one will give 510 we want 512
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++) // loop through sampecounts to get there average
            {
                average += audioSamples[count] * (count + 1); 
                count++;
            }
            average /= count;  // divide sum by there count to get average
            frequencyBand[i] = average * 10;
        }
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(audioSamples, 0, FFTWindow.Blackman);
    }
}
