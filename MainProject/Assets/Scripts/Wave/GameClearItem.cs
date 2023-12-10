using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerModel>().Success();
        }
    }
}
