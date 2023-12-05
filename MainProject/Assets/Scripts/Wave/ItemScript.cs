using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemScript : MonoBehaviour
{
    [SerializeField] private bool isSuccess;
    private MenuInput menuInput;

    void Start()
    {
        menuInput = GameObject.Find("menu").GetComponent<MenuInput>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (menuInput != null)
            {
                if (isSuccess)
                {
                    menuInput.Success();
                }
                else
                {
                    menuInput.GameOver();
                }
            }

            Destroy(gameObject);
        }
    }

    // public void WaveItem(WaveModel waveModel)
    // {
    //     this.waveModel = waveModel;
    // }
}
