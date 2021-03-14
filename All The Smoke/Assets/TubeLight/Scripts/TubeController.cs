using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeController : MonoBehaviour
{

    public Vector3 anglesToRotate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // this.transform.Rotate(new Vector3 (1f, 0f, 0f), anglesToRotate.x * Time.deltaTime, Space.World);
        // this.transform.Rotate(new Vector3 (0f, 1f, 0f), anglesToRotate.y * Time.deltaTime, Space.World);
        // this.transform.Rotate(new Vector3 (0f, 0f, 1f), anglesToRotate.z * Time.deltaTime, Space.World);
        this.transform.RotateAround(new Vector3 (0f, 0f, 0f), new Vector3 (0f, 0f, 1f), 90f * Time.deltaTime);
    }
}
