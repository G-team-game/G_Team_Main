using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    public static UIMgr Instance;


    //public GameObject bullet;//
    [Header("Bullet Speed")]
    public float BulletSpeed;
    [Header("Shot Position")]
    public Transform gunPos;
    [Header("Bullet Left Text")]
    public Text Text_Num;

    private GameObject CurrentBullet;


    public bool isFire;
    private int currentNum;
    void Start()
    {
        Instance = this;
        currentNum = 0;
        isFire = false;
        CurrentBullet = null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentNum == 0)
            {
                StartCoroutine(ChangeTextColor());
            }
            if (isFire&&currentNum>0)
            {
                //GameObject go = Instantiate(bullet, gunPos.position, Quaternion.identity);
                //go.GetComponent<Rigidbody>().velocity = gunPos.transform.forward * BulletSpeed;
                Rigidbody rb = CurrentBullet.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.velocity = gunPos.transform.forward * BulletSpeed;
                CurrentBullet.transform.LookAt(Camera.main.transform.forward * 1000);

                //shot bullet , text -1
                ChangeBulletNum(-1);
                CurrentBullet = null;
                isFire = false;
            }
            
        }
    }

    /// <param name="num"></param>
    public void ChangeBulletNum(int num, GameObject go = null)
    {
        if (go != null)
        {
            CurrentBullet = go;
            isFire = true;
        }
        currentNum += num;

        //is there a bullet or not

        if (currentNum<=0)
        {
            currentNum = 0;
        }
        Text_Num.text = currentNum.ToString();
    }
    public int GetBullet()
    {
        //if (currentNum==0)
        //{
        //    Text_Num.color = Color.red;
        //}
        return currentNum;
    }

    IEnumerator ChangeTextColor()
    {
        Text_Num.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        Text_Num.color = Color.black;
    }
}
