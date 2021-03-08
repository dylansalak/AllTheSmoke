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

    public AudioPeer peer;

    [Tooltip("Which axis the object should rotate around.")]
    public RotationAxis axis = RotationAxis.Z;

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

        DetermineRotation();
    }

    void DetermineRotation()
    {
        if(peer.samples == null)
        {
            Debug.Log("Peer isn't ready yet");
            return;
        }
        
        var rotationChange = peer.samples[sampleNumber] * signalScale;

        //calculate the rotation amount relative to tickrate
        //rotationChange = -(rotationRate / tickRate);

        //create a rotation vector based off of the selected axis
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

