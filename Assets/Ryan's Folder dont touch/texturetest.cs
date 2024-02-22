using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class texturetest : MonoBehaviour
{  
    public GameObject player;
    Vector3 offset;

     Vector3 initialCameraPosition;
    // Start is called before the first frame update
    void Start()
    {
       //initialCameraPosition = cameraLocation.transform.localPosition;
    }

    // Update is called once per frame
    void Update () {
        offset = (player.transform.localPosition);
        GetComponent<Renderer>().material.mainTextureOffset = offset;
        GetComponent<Renderer>().material.mainTextureScale = new Vector2((1/player.transform.localPosition.z * 0.5f), player.transform.localPosition.z * 0.5f);

        
        
    }


}
