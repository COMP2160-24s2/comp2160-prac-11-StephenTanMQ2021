using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleFollower : MonoBehaviour
{
    [SerializeField] Transform marble;
    [SerializeField] Transform secondTarget;

    // Update is called once per frame
    void Update()
    {
        Vector3 midPoint = (marble.position + secondTarget.position)/2;
        transform.position = new Vector3(midPoint.x, 0, midPoint.z);
    }
}
