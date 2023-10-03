using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderEnemy : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        SetWayPoint();
    }
    protected override void Update()
    {
        if (!CheckDashList(this.gameObject)&&isDash)
            isDash = false;
        if (isDash)
        {
            direction = (dashPosition - transform.position).normalized;
            if (Vector3.Distance(transform.position,dashPosition) < 1.0f)
            {
                isDash = false;
                enemyManagement.dashList.Remove(this.gameObject);
            }
        }
        else
        {
            if (CheckDashList(this.gameObject))
            {
                StartDashTowardsPlayer();
            }
            if (Vector3.Distance(transform.position, wanderPosition) < 1f)
            {
                SetWayPoint();
            }
            direction = (wanderPosition - transform.position).normalized;
        }
        base.Update();
    }

    private bool CheckDashList(GameObject obj)
    {
        return enemyManagement.dashList.Contains(obj);
    }

    private void StartDashTowardsPlayer()
    {
        isDash = true;
        dashPosition = playerTransform.position;
    }
}
