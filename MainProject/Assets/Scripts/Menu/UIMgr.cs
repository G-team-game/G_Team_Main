using System.Collections;
using UnityEngine;
using TMPro;
public class UIMgr : MonoBehaviour
{
    public static UIMgr Instance;

    public GameObject player;
    private PlayerMove pm;

    //public GameObject bullet;//
    [Header("Bullet Speed")]
    public float BulletSpeed;
    [Header("Shot Position")]
    public Transform gunPos;
    [Header("Bullet Left Text")]
    public TextMeshProUGUI Text_Num;

    private EnemyBase CurrentBullet;

    public bool isFire;
    private int currentNum;
    void Start()
    {
        // pm = player.GetComponent<PlayerMove>();

        // Instance = this;
        // currentNum = 0;
        // isFire = false;
        // CurrentBullet = null;
    }

    // void Update()
    // {
    //     if (pm.shotButtonState)
    //     {
    //         if (currentNum == 0)
    //         {
    //             StartCoroutine(ChangeTextColor());
    //         }

    //         if (isFire && currentNum > 0)
    //         {
    //             //
    //             CurrentBullet.transform.parent = null;
    //             CurrentBullet.transform.LookAt(Camera.main.transform.forward * 1000);
    //             CurrentBullet.Shoot(BulletSpeed, Camera.main.transform.forward);

    //             ChangeBulletNum(-1);
    //             CurrentBullet = null;
    //             isFire = false;
    //         }
    //     }
    // }


    //     /// <param name="num"></param>
    //     public void ChangeBulletNum(int num, EnemyBase go = null)
    //     {
    //         if (go != null)
    //         {
    //             CurrentBullet = go;
    //             // CurrentBullet.Restraint();
    //             isFire = true;
    //         }

    //         currentNum += num;

    //         //is there a bullet or not

    //         if (currentNum <= 0)
    //         {
    //             currentNum = 0;
    //         }

    //         Text_Num.text = currentNum.ToString();
    //     }

    //     public int GetBullet()
    //     {
    //         return currentNum;
    //     }

    //     IEnumerator ChangeTextColor()
    //     {
    //         Text_Num.color = Color.red;
    //         yield return new WaitForSeconds(0.5f);
    //         Text_Num.color = Color.black;
    //     }
}
