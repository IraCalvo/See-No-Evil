using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showTextInOrder : MonoBehaviour
{
    public GameObject[] textObjects;

    int count = 0;
    
    void OnTriggerEnter(){
        Debug.Log("WORKED");
        textObjects[0].SetActive(true);
    }

     void OnTriggerExit(){
        Debug.Log("WORKED");
        textObjects[0].SetActive(false);
        textObjects[1].SetActive(true);
        StartCoroutine(End());
        
    }

    IEnumerator End()
    {
        // Iterate through each text object
      
        // Display the text object
       
        
        // Wait for a short amount of time
        yield return new WaitForSeconds(5f);
        Debug.Log("YO");
        Application.Quit();
    }
}
