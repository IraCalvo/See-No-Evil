using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class texttureTest2 : MonoBehaviour
{
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        offset = (transform.localPosition);
        GetComponent<Renderer>().material.mainTextureOffset = offset * -1 ;

    }
}
