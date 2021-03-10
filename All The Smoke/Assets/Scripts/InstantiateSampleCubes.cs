using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateSampleCubes : MonoBehaviour
{
    public GameObject cubeFab;
    public AudioPeer peer;

    public float maxScale = 10f;

    private GameObject[] sampleCubes = new GameObject[512]; 
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 512; ++i)
        {
            sampleCubes[i] = Instantiate(
                cubeFab, 
                this.gameObject.transform
            );
            sampleCubes[i].transform.Rotate(i * (360f / 512), 0f, 0f);
            sampleCubes[i].transform.Translate(sampleCubes[i].transform.forward * 100);
            /*new Vector3((float)i, 0f, 0f),
                Quaternion.Euler(i * (360f / 512), 0f, 0f),*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < 512; ++i)
        {
            if(sampleCubes[i] != null)
            {
                sampleCubes[i].transform.localScale = new Vector3(10f, peer.samples[i] * maxScale + 2, 10f);
            }
        }
    }
}
