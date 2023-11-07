//細かい挙動とか埋まっちゃうのとかなんとかしたい
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : EnemyBase
{
    //playerからどこまでの範囲内でランダム座標を決定するか
    [SerializeField] protected float chasingMaxDistance = 15f;
    //playerからどの距離に入ったら直で追っかけるようにするか
    [SerializeField] protected float chasingPlayerDistance = 3f;
    private Vector3 chasePosition;
    protected override void Start()
    {
        base.Start();
        GetRandomPositionNearPlayer();
    }
    protected override void Update()
    {
        if (Vector3.Distance(transform.position, chasePosition) < 1f)
        {
            GetRandomPositionNearPlayer();
        }
        if (Vector3.Distance(transform.position, playerTransform.position) < chasingPlayerDistance)
        {
            direction = (playerTransform.position - transform.position).normalized;
        }
        else
        {
            direction = (chasePosition - transform.position).normalized;
        }
        base.Update();
    }

    protected virtual void GetRandomPositionNearPlayer()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 randomOffset = Random.insideUnitSphere * chasingMaxDistance;
        Vector3 randomposition = playerPosition + randomOffset;
        //if (randomposition.y < 0)
        //{
        //    //地面に埋まんないように座標を正にしてたけど少し処理変えたほうがいいかも
        //    randomposition.y = Mathf.Abs(randomposition.y);
        //}
        Collider[] colliders = Physics.OverlapSphere(randomposition, chasingMaxDistance, collisionLayer);
        if (colliders.Length > 0)
        {
            GetRandomPositionNearPlayer();//もう一回ランダム生成
        }
        chasePosition = randomposition;
    }
}
