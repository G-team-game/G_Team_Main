using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr : MonoBehaviour
{
    public float atktimer = 0;
    public int atkDamage = 5;
    public bool isFire = false;
 
    void Update()
    {
        atktimer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isFire)
        {
            if (atktimer <= 0)
            {
                atktimer = 1;
                other.GetComponent<PlayerHP>().Hit(atkDamage);
            }
        }
    }
   
}
