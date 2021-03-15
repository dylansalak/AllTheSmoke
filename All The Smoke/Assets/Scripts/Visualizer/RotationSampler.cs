using System;
using UnityEngine;

public class RotationSampler : MonoBehaviour
{
    //FIELDS---------------------------------------------------------------------------------------
    public enum RotationAxis {
        X,
        Y,
        Z
    }

    [Tooltip("The peer this rotator gets its samples from.")]
    public AudioPeer peer;

    [Tooltip("The GameObject that is principally seen/rotated by the rotator")]
    public GameObject principal;

    [Tooltip("Which axis the object should rotate around.")]
    public RotationAxis axis = RotationAxis.Z;

    [Tooltip("Whether this object should rotate in a negative direction")]
    public bool oppisiteRotation = false;

    [Tooltip("How high a signal needs to be to prevent the princiapl from being shut off")]
    [Range(0f, 1f)]
    public float signalCutoff = 0.05f;

    [Tooltip("How far (in degrees) the rotator should be allowed to rotate")]
    public float rotationCap = 90f;

    [Tooltip("How much to multiply the recieved signal by")]
    public float signalScale = 100f;

    public int sampleNumber{ get; set; } = 0;

    /*[Tooltip("How fast the object should rotate in DEGREES PER SECOND. Set to a negative number" +
        " for counter-clockwise rotation.")]
    public float rotationRate = 360f;*/

    //the global tickrate of 60 (it doesn't seem accessible as a readonly value)
    //private const float tickRate = 60f;

    //transform of rotating object
    private Transform trans;

    //the vector object needed to perform the rotation
    private Vector3 rotationEulers;

    private Vector3 rotationMin;
    private Vector3 rotationMax;
    //calculated degrees of rotation per simulation tick
    //private float rotationChange; 
    //METHODS--------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        //grab transform object
        trans = this.gameObject.transform;

        //set a default rotation
        rotationEulers = new Vector3(0f, 0f, 0f);

        rotationMin = new Vector3(0f, 0f, 0f);
        switch(axis)
        {
            case RotationAxis.X:
                rotationMax = new Vector3(rotationCap, 0f, 0f);
                break;

            case RotationAxis.Y:
                rotationMax= new Vector3(0f, rotationCap, 0f);
                break;

            case RotationAxis.Z:
                rotationMax = new Vector3(0f, 0f, rotationCap);
                break;

            default:
                rotationMax = new Vector3(0f, 0f, 0f);
                Debug.LogWarning("ConstantRotation: Rotation Axis could not be set correctly");
                break;
        }

        //invert rotation when approprite
        if(oppisiteRotation == true)
        {
            rotationMax *= -1.0f;
        }

        if(principal == null)
        {
            principal = this.gameObject.transform.GetChild(0).gameObject;
        }

        DetermineRotation();
    }

    void DetermineRotation()
    {
        if(peer == null || peer.samples == null)
        {
            Debug.Log("Peer isn't ready yet");
            peer = GameObject.FindWithTag("Peer")?.GetComponent<AudioPeer>();
        }
        
        //var rotationChange = peer.samples[sampleNumber] * signalScale;

        //calculate the rotation amount relative to tickrate
        //rotationChange = -(rotationRate / tickRate);

        //create a rotation vector based off of the selected axis
        /*
        switch(axis){
            case RotationAxis.X:
                rotationEulers = new Vector3(rotationChange, 0f, 0f);
                break;

            case RotationAxis.Y:
                rotationEulers = new Vector3(0f, rotationChange, 0f);
                break;

            case RotationAxis.Z:
                rotationEulers = new Vector3(0f, 0f, rotationChange);
                break;

            default:
                rotationEulers = new Vector3(0f, 0f, 0f);
                Debug.LogWarning("ConstantRotation: Rotation Axis could not be set correctly");
                break;
            
        }*/

        rotationEulers = Vector3.Lerp(rotationMin, rotationMax, peer.samples[sampleNumber]);

        if(peer.samples[sampleNumber] < signalCutoff)
        {
            principal.SetActive(false);
        }
        else
        {
            principal.SetActive(true);
        }
    }

    // FixedUpdate is called once per simulation tick
    void Update()
    {
        //trans.Rotate(rotationEulers, Space.Self);
        trans.localEulerAngles = rotationEulers;

        DetermineRotation();
    }
}

