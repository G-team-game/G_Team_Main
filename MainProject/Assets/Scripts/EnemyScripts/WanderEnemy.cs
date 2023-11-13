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
    //ランダム座標を決定するときにenemyからどこまでの範囲で座標を決めるか
    [SerializeField] protected float wanderingDistance = 30f;
    protected override void Start()
    {
        base.Start();
        SetWayPoint();
    }
    protected override void Update()
    {
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
        Quaternion targetRotation = Quaternion.LookRotation(dashPosition - transform.position);
        float rotationTime = 0.0f;
        while(rotationTime<3.0f)
        {
            rotationTime+=Time.deltaTime;
            float t = Mathf.Clamp01(rotationTime / 3.0f);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        float moveTime = 0.0f;
        while(moveTime<3.0f)
        {
            moveTime+=Time.deltaTime;
            transform.Translate(Vector3.forward*dashSpeed*Time.deltaTime);
            yield return null;

            if(CheckCollision())
            {
                //突進中何かにぶつかったときの処理とか
                isStop = true;
                break;
            }
        }
        gameObject.GetComponent<Renderer>().material.color = Color.gray;
        isDash = false;
        SetWayPoint();
        yield return new WaitForSeconds(2.0f);
        enemyManagement.dashList.Remove(this.gameObject);
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        isCoroutine = false;
        isCheckList = true;

    }

    protected virtual void SetWayPoint()
    {
        Vector3 randomPosition = GenerateRandomPosition();
        wanderPosition = randomPosition;

    }
    private Vector3 GenerateRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderingDistance;
        randomDirection += transform.position; // 現在の位置を起点にする
        randomDirection.y = Mathf.Abs(randomDirection.y);

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
        //突進してるときに何かにぶつかったら～っていう判定用
        return false;
    }

}
