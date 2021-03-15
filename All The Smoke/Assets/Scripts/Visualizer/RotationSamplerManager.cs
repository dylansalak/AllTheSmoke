using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSamplerManager : MonoBehaviour
{
    public AudioPeer peer;

    [Tooltip("0KHz - 60KHz")]
    public GameObject[] SubbassRotators;

    [Tooltip("60KHz - 250KHz")]
    public GameObject[] BassRotators;

    [Tooltip("250KHz - 500KHz")]
    public GameObject[] LowerMidrangeRotators;

    [Tooltip("500KHz - 2000KHz")]
    public GameObject[] MidrangeRotators;

    [Tooltip("2000KHz - 4000KHz")]
    public GameObject[] UpperMidrangeRotators;

    [Tooltip("4000KHz - 6000KHz")]
    public GameObject[] PresenceRotators;

    [Tooltip("6000KHz - 20000KHz")]
    public GameObject[] BrillianceRotators;

    private readonly float[] sampleRates = new float[] 
    {
        0.003f,
        0.012f,
        0.025f,
        0.1f,
        0.2f,
        0.3f,
        0.7f
    };

    private int[] sampleStarts;

    private int[] sampleEnds;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initialization starting");
        /*initialize sampleStart and sampleEnd arrays. Doing manually because there's
        always going to be sorted out like this because that's how sound works.*/
        sampleStarts = new int[]
        {
            0,
            Mathf.CeilToInt(peer.sampleScale * sampleRates[0]),
            Mathf.CeilToInt(peer.sampleScale * sampleRates[1]),
            Mathf.CeilToInt(peer.sampleScale * sampleRates[2]),
            Mathf.CeilToInt(peer.sampleScale * sampleRates[3]),
            Mathf.CeilToInt(peer.sampleScale * sampleRates[4]),
            Mathf.CeilToInt(peer.sampleScale * sampleRates[5]),
        };
        sampleEnds = new int[]
        {
            Mathf.CeilToInt(peer.sampleScale * sampleRates[0]) - 1,
            Mathf.CeilToInt(peer.sampleScale * sampleRates[1]) - 1,
            Mathf.CeilToInt(peer.sampleScale * sampleRates[2]) - 1,
            Mathf.CeilToInt(peer.sampleScale * sampleRates[3]) - 1,
            Mathf.CeilToInt(peer.sampleScale * sampleRates[4]) - 1,
            Mathf.CeilToInt(peer.sampleScale * sampleRates[5]) - 1,
            peer.sampleScale - 1
        };

        if(peer == null) {Debug.LogError("Not ready to assign! No peer!");}

        //Now to initialize all of the rotators on a frequency band-by-band basis
        InitializeFrequencyBand(SubbassRotators, sampleStarts[0], sampleEnds[0]);

        InitializeFrequencyBand(BassRotators, sampleStarts[1], sampleEnds[1]);

        InitializeFrequencyBand(LowerMidrangeRotators, sampleStarts[2], sampleEnds[2]);

        InitializeFrequencyBand(MidrangeRotators, sampleStarts[3], sampleEnds[3]);

        InitializeFrequencyBand(UpperMidrangeRotators, sampleStarts[4], sampleEnds[4]);

        InitializeFrequencyBand(PresenceRotators, sampleStarts[5], sampleEnds[5]);

        InitializeFrequencyBand(BrillianceRotators, sampleStarts[6], sampleEnds[6]);

        Debug.Log("Initialization Ready");
    }

    void InitializeFrequencyBand(GameObject[] rotators, int startSample, int endSample)
    {
        //start with some basic values
        int desiredRotatorCount = endSample - startSample;

        //If there were no rotators assigned to this band, just don't bother
        if(rotators.Length == 0)
        {
            return;
        }

        /*Check to see how many rotators we have compared to samples*/
        //If we have fewer rotators than samples or they have the same number...
        if(rotators.Length <= desiredRotatorCount)
        {
            /*//cut out the low and high samples and use a smaller range
            //double check to make sure it's even or odd too because C# division drops remainder
            int workingStart;
            int workingEnd;
            //int difference = desiredRotatorCount - rotators.Length;
            //we also need to split up the samples based on whether or notthe amount of rotators is even or odd
            //if odd, then just base the range from the middle of eligible samples
            if(desiredRotatorCount % 2 != 0)
            {
                if(rotators.Length % 2 == 0)
                {
                    workingStart = (desiredRotatorCount / 2) - (rotators.Length / 2) + 2;
                    workingEnd = (desiredRotatorCount / 2) + (rotators.Length / 2) + 1;
                }
                else
                {
                    workingStart = (desiredRotatorCount / 2) - (rotators.Length / 2) + 1;
                    //the +1 shunts the remained into the end
                    workingEnd = (desiredRotatorCount / 2) + (rotators.Length / 2) + 2;
                }
                
            }
            //if even, we'll need to guess. Because C# drops remainder for int division, the lower
            //end of the true middle will be on the left
            {
                if(rotators.Length % 2 == 0)
                {
                    
                    //thanks, even numbers
                    workingStart = (desiredRotatorCount / 2) - (rotators.Length / 2) + 1;
                    workingEnd = (desiredRotatorCount / 2) + (rotators.Length / 2);
                }
                else
                {
                    //add one to account for the fact that desiredRotatorCount is on this side of the middle
                    //and add one more to account for remainder drop
                    workingStart = (desiredRotatorCount / 2) - (rotators.Length / 2) + 2;
                    //subtract one to account for shifting the remainder onto this side
                    workingEnd = (desiredRotatorCount / 2) + (rotators.Length / 2) - 1;
                }
            }
            
            //we then AssignOneToOne, just with a smaller range
            Debug.Log($"Too many samples for rotators, decreasing samples to {workingStart} to {workingEnd}");*/

            //Assign the One-to-One
            Debug.Log("Less than or equal rotators for samples");
            AssignOneToOne(rotators, startSample, endSample);
        }
        //If we have more rotators than samples...
        else
        {
            //repeat sample values across rotators, AND CUT THE REMAINDER
            //sorry, I couldn't think of a better way...
            Debug.Log("Too many rotators for samples, duplicating samples");
            AssignDuplication(rotators, startSample, endSample, (endSample - startSample) / rotators.Length);
        }
    }

    void AssignOneToOne(GameObject[] rotators, int startSample, int endSample)
    {
        GameObject rotator;
        RotationSampler sampler;

        double slope;
        int finalSampleValue;

        for(int i = 0; i < rotators.Length; ++i)
        {
            //get the deisred rotator and sampler
            rotator = rotators[i];
            sampler = rotator.GetComponent<RotationSampler>();

            //check to see if the rotator has a RotationSampler
            if(sampler == null)
            {
                //and give it one if it doesn't
                sampler = rotator.AddComponent<RotationSampler>();
            }
            //force sampler reassignment
            //sampler = rotator.AddComponent<RotationSampler>();

            //Give the sampler the peer and its assigned sample number to read
            sampler.peer = peer;

            /*This range mapping comes from here:
            https://stackoverflow.com/questions/5731863/mapping-a-numeric-range-onto-another
            It heavliy favors the front end of the range though, which may not be desired*/
            slope = (endSample - startSample) / (rotators.Length - 0.0);
            finalSampleValue = Convert.ToInt32(Math.Round(startSample + slope * (i - 0.0)));

            //Debug.Log($"Assigning sample {finalSampleValue} to rotator {i} from range {startSample} to {endSample}");
            sampler.sampleNumber = i;
        }

        /*for(int i = startSample; i <= endSample; ++i)
        {
            //get the deisred rotator and sampler
            Debug.Log($"{i} minus startSample of {startSample} is {i - startSample} (endSample is {endSample})");
            rotator = rotators[i - startSample];
            sampler = rotator.GetComponent<RotationSampler>();
            
            //check to see if the rotator has a RotationSampler
            if(sampler == null)
            {
                //and give it one if it doesn't
                sampler = rotator.AddComponent<RotationSampler>();
            }

            //Give the sampler the peer and its assigned sample number to read
            sampler.peer = peer;
            sampler.sampleNumber = i;
        }*/
    }

    void AssignDuplication(GameObject[] rotators, int startSample, int endSample, int duplicationSize)
    {
        GameObject rotator;
        RotationSampler sampler;

        //get the actual end point based on sampling sizes
        int workingEnd = (duplicationSize * rotators.Length) + startSample;
        //for each sample
        for(int i = startSample; i <= workingEnd; ++i)
        {
            //assign it to a number of rotators, so each sample gets the same(ish) amount of rotators
            for(int j = 0; j < duplicationSize; ++j)
            {
                //get the deisred rotator and sampler
                rotator = rotators[i - startSample];
                sampler = rotator.GetComponent<RotationSampler>();
            
                //check to see if the rotator has a RotationSampler
                if(sampler == null)
                {
                    //and give it one if it doesn't
                    sampler = rotator.AddComponent<RotationSampler>();
                }
                //force sampler reassignment
                //sampler = rotator.AddComponent<RotationSampler>();

                //Give the sampler the peer and its assigned sample number to read
                sampler.peer = peer;
                sampler.sampleNumber = i;
            }
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    /*
    How this script will work:
    A bunch of lists of GameObjects. These are the things that rotate
    There will be multiple lists, one for each freqeuncy band
    An AudioPeer to get the samples from
    Each item gets a RotationSampler component, if it doesn't have one already
        that's gonna be the thing that actually rotates
        it's gonna have a public method that's gonna get called via message
    Each rotation object will be passed a sample number based on how many objects there are for
        that group
    If the number of samples in the band matches the numbers of objects, it will be 1-to-1
    If there are more samples than objects, the samples at the front and back will be cut out,
        since they (probably?) contain the least interesting information
    If there are more objects than samples, divide object number by sample number and assign samples
        in groups of that size. Any remained will be given the final sample number
    */
}
