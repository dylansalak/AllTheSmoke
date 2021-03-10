using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManyBandsAutomator : MonoBehaviour
{
    public GameObject visualizer;
    public GameObject rotatorFab;
    public RotationSamplerManager manager;
    public int rotatorsPerBand;

    private int distCount = 0;
    void Awake()
    {
        manager.SubbassRotators = SetArray();
        manager.BassRotators = SetArray();
        manager.LowerMidrangeRotators = SetArray();
        manager.MidrangeRotators = SetArray();
        manager.UpperMidrangeRotators = SetArray();
        manager.PresenceRotators = SetArray();
        manager.BrillianceRotators = SetArray();

        Debug.Log("Arrays ready");
        visualizer.SetActive(true);
    }

    GameObject[] SetArray()
    {
        GameObject[] rotators = new GameObject[rotatorsPerBand];
        for(int i = 0; i < rotatorsPerBand; ++i)
        {
            rotators[i] = Instantiate(rotatorFab, new Vector3(0f, 0f, 2f * distCount), Quaternion.identity, this.gameObject.transform);
            ++distCount;
        }

        return rotators;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
