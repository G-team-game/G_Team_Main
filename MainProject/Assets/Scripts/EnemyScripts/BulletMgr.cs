using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMgr : MonoBehaviour
{
    public float moveSpeed = 10;
  
    void Start()
    {
        //StartCoroutine(DestoryBullet());
    }

    private void Update()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag== "Enemy")
        {
            Debug.Log("Enemy Hit");
            other.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
    IEnumerator DestoryBullet()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
