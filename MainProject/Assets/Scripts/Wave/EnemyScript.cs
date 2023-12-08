using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //突進はspawnerのリストからランダムで指定数分選んでさせる感じで
    //突進の仕組み的には
    //・プレイヤーの座標取得→そこにダッシュ→座標着いたらストップ
    //かなぁって感じです。

    [SerializeField] float speed = 3f;//スピード
    [SerializeField] float rotationSpeed = 5f;//旋回スピード
    //[SerializeField] float DistanceFromAB = 10f;使ってないので一旦消し
    [SerializeField] float wanderingDistance = 30f;//ランダムで移動するときにenemyからの距離内で生成するためのやつ
    [SerializeField] float ChasingMaxDistance = 15f;//追跡処理中にランダムで座標指定するときのプレイヤーからの距離
    [SerializeField] float playerChaseDistance = 3f;//プレイヤーを直で追っかけるモードに移行するときの距離 

    private Transform player; // プレイヤーの位置
    private Vector3 targetPosition; // 現在の目的地
    private bool isChasing = false; // 追跡モードかどうか
    private Transform rangeA;
    private Transform rangeB;

    private LayerMask collisionLayer; // 衝突判定を行うレイヤーマスク
    private Vector3 ChasingRandomPosition;

    public EnemySpawn enemySpawn;

    private void Start()
    {
        rangeA = GameObject.Find("ChaseRangeA").transform;
        rangeB = GameObject.Find("ChaseRangeB").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemySpawn = EnemySpawn.Instance;
        if (enemySpawn == null)
        {
            Debug.LogError("EnemySpawn not found.");
        }


        if (!isChasing)
            SetWayPoint();
        else if (isChasing)
            ChasingRandomPosition = GetRandomPositionNearPlayer();
    }

    private void Update()
    {
        Vector3 direction;
        if (isChasing)
        {
            if (Vector3.Distance(transform.position, ChasingRandomPosition) < 1f)
            {
                ChasingRandomPosition = GetRandomPositionNearPlayer();
            }

            //Debug.Log(ChasingRandomPosition);
            if (Vector3.Distance(transform.position, player.position) < playerChaseDistance)
            {
                direction = (player.position - transform.position).normalized;
            }
            else
            {
                direction = (ChasingRandomPosition - transform.position).normalized;
            }
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);


            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else if (!isChasing)// 徘徊する処理
        {
            if (CheckDashList(this.gameObject))
            {
                Debug.Log("ダッシュ中" + this.gameObject.name);
            }
            else if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                SetWayPoint();
            }
            direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        }
    }

    private Vector3 GetRandomPositionNearPlayer()
    {
        Vector3 playerPosition = player.position;
        Vector3 randomOffset = Random.insideUnitSphere * ChasingMaxDistance;
        Vector3 randomposition = playerPosition + randomOffset;
        if (randomposition.y < 0)
        {
            randomposition.y = Mathf.Abs(randomposition.y);//地面より下に行っちゃわないようにy座標を正に
        }
        Collider[] colliders = Physics.OverlapSphere(randomposition, ChasingMaxDistance, collisionLayer);
        if (colliders.Length > 0)
        {
            return GetRandomPositionNearPlayer();//もう一回ランダム生成
        }
        return randomposition;
    }
    private void SetWayPoint()
    {
        Vector3 randomPosition = GenerateRandomPosition();
        Debug.Log("Random Position: " + randomPosition + this.gameObject.name);//ランダム座標確認用
        targetPosition = randomPosition;

    }

    private Vector3 GenerateRandomPosition()
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
    private bool CheckDashList(GameObject obj)
    {
        return enemySpawn.DashList.Contains(obj);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            //Debug.Log("Playerに当たった!");
        }
    }

    public void EnemyChaseSetActive()//EnemySpawn側から呼び出して追跡モードtrueにするよーってやつです。
    {
        isChasing = true;
        //Debug.Log("Active!" + isChasing);
    }
}
