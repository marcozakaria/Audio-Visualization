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

    // to help us get a value between(0,1) to be used in any thing ex: lighting scale...
    float[] frequencyBandHighest = new float[8];  
    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

    public static float amplitude, amplitudeBuffer; // amplitude gits average  from our 8 bands
    float amplitudeHieghest;

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
        CreateAudioBands();
    }

    void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }
        if (currentAmplitude > amplitudeHieghest)
        {
            amplitudeHieghest = currentAmplitude;
        }
        amplitude = currentAmplitude / amplitudeHieghest;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHieghest;
    }

    void CreateAudioBands()  // help us get value between 0 and 1 
    {
        for (int i = 0; i < 8; i++)
        {
            if (frequencyBand[i] > frequencyBandHighest[i])
            {
                frequencyBandHighest[i] = frequencyBand[i];
            }
            audioBand[i] = (frequencyBand[i] / frequencyBandHighest[i]); // get ratio
            audioBandBuffer[i] = (bandBuffer[i] / frequencyBandHighest[i]);
        }
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
