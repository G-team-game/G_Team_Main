using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : EnemyBase
{
    [SerializeField] float resetDistanse;
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
}
