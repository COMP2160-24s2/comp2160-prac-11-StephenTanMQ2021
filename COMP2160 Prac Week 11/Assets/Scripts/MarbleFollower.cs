using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleFollower : MonoBehaviour
{
    [SerializeField] Transform marble;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = marble.position;
    }
}
