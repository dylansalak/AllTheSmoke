using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSamplerFactory : MonoBehaviour
{
    public GameObject samplerFab;
    public AudioPeer peer;

    private int numObj;
    private GameObject[] samplers;
    // Start is called before the first frame update
    void Start()
    {
        samplers = new GameObject[peer.sampleScale];

        for(int i = 0; i < peer.sampleScale; ++i)
        {
            samplers[i] = Instantiate(
                samplerFab,
                new Vector3(0f, 0f, 5f * i),
                Quaternion.identity,
                this.gameObject.transform
            );
            samplers[i].GetComponent<RotationSampler>().peer = peer;
            samplers[i].GetComponent<RotationSampler>().sampleNumber = i;
        }
    }
}
