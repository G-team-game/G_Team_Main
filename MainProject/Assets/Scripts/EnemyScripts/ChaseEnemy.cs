//rayで障害物判定作ってる途中
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : EnemyBase
{
    //playerからどこまでの範囲内でランダム座標を決定するか
    [SerializeField] float chasingMaxDistance = 15f;
    //playerからどの距離に入ったら直で追っかけるようにするか
    [SerializeField] float chasingPlayerDistance = 3f;
    //Playerとどの距離以上になったら座標を再設定するか
    [SerializeField] float maxPlayerDistance = 15f;
    private Vector3 chasePosition;
    protected override void Start()
    {
        base.Start();
        GetRandomPositionNearPlayer();
    }
    protected override void Update()
    {
        if(Physics.Raycast(transform.position,chasePosition, out RaycastHit hit, collisionLayer))
        {
            Debug.Log("障害物を検知");
            GetRandomPositionNearPlayer();
        }
        if (Vector3.Distance(transform.position, chasePosition) < 1f||Vector3.Distance(transform.position,playerTransform.position)>maxPlayerDistance)
        {
            GetRandomPositionNearPlayer();
        }

        if (Vector3.Distance(transform.position, playerTransform.position) < chasingPlayerDistance)
        {
            direction = (playerTransform.position - transform.position).normalized;
            GetRandomPositionNearPlayer();
        }
        else
        {
            direction = (chasePosition - transform.position).normalized;
        }
        Debug.DrawRay(transform.position,direction);
        base.Update();
    }

    private void GetRandomPositionNearPlayer()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 randomOffset = Random.insideUnitSphere * chasingMaxDistance;
        Vector3 randomPosition = playerPosition + randomOffset;
        //if (randomposition.y < 0)
        //{
        //    //地面に埋まんないように座標を正にしてたけど少し処理変えたほうがいいかも
        //    randomposition.y = Mathf.Abs(randomposition.y);
        //}
        Collider[] colliders = Physics.OverlapSphere(randomPosition, chasingMaxDistance, collisionLayer);
        if (colliders.Length > 0|| Physics.Linecast(transform.position, randomPosition, out RaycastHit hit, collisionLayer))
        {
            Debug.Log("障害物を検知");
            GetRandomPositionNearPlayer();//もう一回ランダム生成
        }
        chasePosition = randomPosition;
    }
}
