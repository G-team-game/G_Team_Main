using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerModel>().RemoveHp(100);
        }
    }
}