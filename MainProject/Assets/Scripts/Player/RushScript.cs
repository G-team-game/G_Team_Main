using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using KanKikuchi.AudioManager;
public class RushScript : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SEManager.Instance.Play(SEPath.KILL);
        }
    }
}