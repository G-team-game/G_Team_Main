using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    [Header("Scale")]
    public float Scale;

    public float atktimer = 0;
    public int atkDamage = 5;
    private Vector3 targetPos;
    public bool isFire = false;
    void Start()
    {
        
    }

    void Update()
    {
        atktimer -= Time.deltaTime;
    }

    /// <summary>
    /// change Enemy to bullet
    /// </summary>
    public void ChangeScale()
    {
        isFire = true;
        StartCoroutine(DestoryEnemy());
    }
    IEnumerator DestoryEnemy()
    {
        yield return new WaitForSeconds(1f);//wait 1s
        //this.GetComponent<SphereCollider>().enabled = false;
        float t = 0;
        targetPos = player.transform.position;
       // this.transform.localScale = new Vector3(Scale, Scale, Scale);
        while (t<1)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPos, t);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * Scale, t);
            t += Time.deltaTime*0.01f;
            yield return 0;
        }

        //UIMgr.Instance.isFire = true;

        this.gameObject.transform.SetParent(UIMgr.Instance.gunPos);
   
        transform.localPosition = new Vector3(0, 0, 0.1f);
        //yield return new WaitForSeconds(0.5f);
        
        UIMgr.Instance.ChangeBulletNum(1, this.gameObject);
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
