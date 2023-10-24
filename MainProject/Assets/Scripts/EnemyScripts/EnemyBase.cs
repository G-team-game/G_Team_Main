using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float speed = 3f;
    [SerializeField] protected float rotationSpeed = 5f;
    [SerializeField] protected float dashSpeed = 10f;
    [SerializeField] protected float wanderingDistance = 30f;
    [SerializeField] protected float chasingMaxDistance = 15f;
    [SerializeField] protected float chasingPlayerDistance = 3f;

    protected Transform playerTransform;
    protected Vector3 wanderPosition;
    protected Vector3 chasePosition;
    protected Vector3 dashPosition;
    protected Vector3 direction;
    protected Transform rangeA;
    protected Transform rangeB;

    protected LayerMask collisionLayer;
    protected EnemyManagement enemyManagement;

    protected bool isDash = false;

    protected virtual void Start()
    {
        enemyManagement=EnemyManagement.Instance;
        if (enemyManagement == null)
        {
            Debug.LogError("EnemyManager not found.");
        }

        rangeA = GameObject.Find("ChaseRangeA").transform;
        rangeB = GameObject.Find("ChaseRangeB").transform;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected virtual void SetWayPoint()//徘徊
    {
        Vector3 randomPosition = GenerateRandomPosition();
        //Debug.Log("Random Position: " + randomPosition + this.gameObject.name);
        wanderPosition = randomPosition;

    }
    private Vector3 GenerateRandomPosition()//徘徊
    {
        //float minX = Mathf.Min(rangeA.position.x, rangeB.position.x);
        //float maxX = Mathf.Max(rangeA.position.x, rangeB.position.x);

        //float minY = Mathf.Min(rangeA.position.y, rangeB.position.y);
        //float maxY = Mathf.Max(rangeA.position.y, rangeB.position.y);

        //float minZ = Mathf.Min(rangeA.position.z, rangeB.position.z);
        //float maxZ = Mathf.Max(rangeA.position.z, rangeB.position.z);

        //float randomX = Random.Range(minX, maxX);
        //float randomY = Random.Range(minY, maxY);
        //float randomZ = Random.Range(minZ, maxZ);
        //Vector3 randomDirection = new Vector3(randomX, randomY, randomZ);
        //return randomDirection;

        //今は下のenemyからある程度の距離(wanderingDistance)の間でランダムで目的地設定しています。
        //上でコメントアウトしてるのがステージ上のrangeAとrangeBの間で完全ランダムで決まる方です。
        //一応どっちも使うかもしれないので残してます。
        Vector3 randomDirection = Random.insideUnitSphere * wanderingDistance;
        randomDirection += transform.position; // 現在の位置を起点にする
        randomDirection.y = Mathf.Abs(randomDirection.y); //地面に埋まっちゃうのでy座標を正に

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
    protected virtual void GetRandomPositionNearPlayer()//追跡
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 randomOffset = Random.insideUnitSphere * chasingMaxDistance;
        Vector3 randomposition = playerPosition + randomOffset;
        if (randomposition.y < 0)
        {
            randomposition.y = Mathf.Abs(randomposition.y);//地面より下に行っちゃわないようにy座標を正に
        }
        Collider[] colliders = Physics.OverlapSphere(randomposition, chasingMaxDistance, collisionLayer);
        if (colliders.Length > 0)
        {
            GetRandomPositionNearPlayer();//もう一回ランダム生成
        }
        chasePosition= randomposition;
    }
}
