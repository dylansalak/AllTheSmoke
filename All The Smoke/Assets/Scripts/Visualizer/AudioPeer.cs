using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource audioSource;
    public int sampleScale = 512;
    
    public float[] samples{ get; protected set; }

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        if(IsValidScale(sampleScale))
        {
            this.samples = new float[sampleScale];
        }
        else
        {
            Debug.LogError("AudioPeer: sampleScale is either less than 64, greater than 8192, or not a power of 2. None of these 3 things are allowed.");
        }

        GetSpectrumAudioSource();
        Debug.Log("Peer ready");
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
    }

    bool IsValidScale(int x)
    {
        return (x >= 64) && (x <= 8192) && ((x & (x - 1)) == 0);
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
    }
}
