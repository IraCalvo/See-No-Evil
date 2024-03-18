using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pNodeCheck : MonoBehaviour
{

    EnemyBehavior eScript;
    // Start is called before the first frame update
    void Start()
    {
        eScript = GameObject.Find("Enemy").GetComponent<EnemyBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col){
        Debug.Log(col.gameObject);
        if (col.CompareTag("pNode")){   
            Debug.Log(col.gameObject);    
            eScript.addToPeakingLocations(col.gameObject);
        }
    }
    
    private void OnTriggerExit(Collider col){        
        if(col.CompareTag("pNode")){  
            Debug.Log("removed");      
            eScript.removeFromPeakingLocations(col.gameObject);
        }
    }
}