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
            Debug.Log($"assiging peer {peer.name} to {samplers[i].name}");

            var sampler = samplers[i].GetComponent<RotationSampler>();
            if(sampler == null)
            {
                Debug.LogWarning("Sampler ain't ready");
            }
            sampler.peer = peer;
            if(sampler.peer == null)
            {
                Debug.LogWarning("Assignment failed");
            }
            sampler.sampleNumber = i;
        }

        //redundant peer setting
        for(int i = 0; i < peer.sampleScale; ++i)
        {
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<RotationSampler>().peer = peer;
        }
    }

    
}
