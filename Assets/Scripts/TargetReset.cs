using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetReset : MonoBehaviour
{
    public GameObject playerTargetObject;

    // Start is called before the first frame update
    void Start()
    {
        playerTargetObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TargetReset")
        {
            playerTargetObject.SetActive(false);
        }
    }
}
