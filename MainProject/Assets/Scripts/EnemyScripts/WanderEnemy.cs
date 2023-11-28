//今はそのまま突っ込むけど壁とかにぶつかったら止まるようにする？
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderEnemy : EnemyBase
{
    bool isForwardDashing = false;
    bool isCheckList = true;
    bool isCoroutine = false;
    bool isStop=false;
    private Vector3 wanderPosition;
    private Vector3 dashPosition;
    private float raycastDistance=5f;
    //ランダム座標を決定するときにenemyからどこまでの範囲で座標を決めるか
    [SerializeField] protected float wanderingDistance = 10f;

    [SerializeField] float rotationTime = 2.0f;
    [SerializeField] float dashTime = 5.0f;
    protected override void Start()
    {
        base.Start();
        SetWayPoint();
    }
    protected override void Update()
    {
        //Ray ray = new Ray(transform.position, transform.forward);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit, raycastDistance))
        //{
        //    Debug.Log("障害物が検出されました。");
        //    Debug.Log(hit.collider.gameObject.name);
        //    if(hit.collider.gameObject.name!="Player")
        //    {
        //        SetWayPoint();
        //    }
        //}
        if (isDash && !isCoroutine)
        {
            isCoroutine = true;
            isCheckList = false;
            StartCoroutine(dashCoroutine());
        }
        else if(!isDash)
        {
            if (CheckDashList(this.gameObject) && isCheckList)
            {
                StartDashTowardsPlayer();
            }
            if (Vector3.Distance(transform.position, wanderPosition) < 1.0f)
            {
                SetWayPoint();
            }
            direction = (wanderPosition - transform.position).normalized;
            base.Update();
        }
    }

    private bool CheckDashList(GameObject obj)
    {
        return enemyManagement.dashList.Contains(obj);
    }

    private void StartDashTowardsPlayer()
    {
        isDash = true;
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        dashPosition = playerTransform.position;
    }
    IEnumerator dashCoroutine()
    {
        Quaternion startRotation=transform.rotation;
        float timeCount = 0.0f;
        while(timeCount<rotationTime)//その場で止まってplayerが視界に入れた時の座標の方向く
        {
            timeCount+=Time.deltaTime;
            float t = Mathf.Clamp01(timeCount / rotationTime);
            Quaternion targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        timeCount = 0.0f;
        while(timeCount<dashTime)//突っ込んでくる
        {
            timeCount+=Time.deltaTime;
            transform.Translate(Vector3.forward*dashSpeed*Time.deltaTime);
            yield return null;

            if(CheckCollision())
            {
                //突進中何かにぶつかったときの処理とか
                isStop = true;
                break;
            }
        }
        gameObject.GetComponent<Renderer>().material.color = Color.gray;//突進終わった後の待機時間的なの
        isDash = false;
        SetWayPoint();
        yield return new WaitForSeconds(2.0f);
        enemyManagement.dashList.Remove(this.gameObject);//リストから消して元の状態徘徊に戻す
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        isCoroutine = false;
        isCheckList = true;

    }

    private void SetWayPoint()//徘徊するときの位置決め
    {
        Vector3 randomPosition = GenerateRandomPosition();
        wanderPosition = randomPosition;

    }
    private Vector3 GenerateRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderingDistance;
        randomDirection.y = Mathf.Abs(randomDirection.y);
        Vector3 newPosition = transform.position + randomDirection;
        Vector3 rayStart = transform.position;
        RaycastHit hit;
        if(Physics.Raycast(rayStart,newPosition.normalized,out hit,wanderingDistance))
        {
            Debug.Log("再生成");
            Debug.DrawRay(rayStart, newPosition * wanderingDistance, Color.red, 10.0f);
            return GenerateRandomPosition();
        }
        //Debug.DrawRay(rayStart, newPosition * wanderingDistance, Color.red, 10.0f);
        // 確認する範囲の最小値と最大値を求める
        Vector3 minRange = new Vector3(
            Mathf.Min(rangeA.position.x, rangeB.position.x),
            Mathf.Min(rangeA.position.y, rangeB.position.y),
            Mathf.Min(rangeA.position.z, rangeB.position.z)
        );

        Vector3 maxRange = new Vector3(
            Mathf.Max(rangeA.position.x, rangeB.position.x),
            Mathf.Max(rangeA.position.y, rangeB.position.y),
            Mathf.Max(rangeA.position.z, rangeB.position.z)
        );

        // 範囲内にランダムな座標があるか確認する
        if (randomDirection.x >= minRange.x && randomDirection.x <= maxRange.x &&
            randomDirection.y >= minRange.y && randomDirection.y <= maxRange.y &&
            randomDirection.z >= minRange.z && randomDirection.z <= maxRange.z)
        {
            return randomDirection; // 範囲内の場合はそのまま返す
        }
        else
        {
            // 範囲外の場合は再度生成する
            return GenerateRandomPosition();
        }
    }

    private bool CheckCollision()
    {
        //突進してるときに何かにぶつかったら～っていう判定用(まだ実装してない)
        return false;
    }

}
