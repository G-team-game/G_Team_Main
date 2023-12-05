using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemscript1 : MonoBehaviour
{   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    
}
