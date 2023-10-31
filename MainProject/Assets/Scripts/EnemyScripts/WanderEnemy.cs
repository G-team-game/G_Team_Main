using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderEnemy : EnemyBase
{
    bool isWaiting = false;
    bool isCheckList = true;
    protected override void Start()
    {
        base.Start();
        SetWayPoint();
    }
    protected override void Update()
    {
        if (isWaiting)
        {
            transform.Translate(Vector3.forward*dashSpeed*Time.deltaTime);
            Debug.Log("ë“ã@íÜÅc");
        }
        else
        {
            if (isDash)
            {
                direction = (dashPosition - transform.position).normalized;
                if (Vector3.Distance(transform.position, dashPosition) < 0.1f)
                {
                    StartCoroutine(waitForSeconds());
                }
            }
            else
            {
                if (CheckDashList(this.gameObject)&&isCheckList)
                {
                    StartDashTowardsPlayer();
                }
                if (Vector3.Distance(transform.position, wanderPosition) < 1.0f)
                {
                    SetWayPoint();
                }
                direction = (wanderPosition - transform.position).normalized;
            }
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
    IEnumerator waitForSeconds()
    {
        isWaiting = true;
        isCheckList = false;
        enemyManagement.dashList.Remove(this.gameObject);
        gameObject.GetComponent<Renderer>().material.color = Color.black;
        yield return new WaitForSeconds(1.0f);
        gameObject.GetComponent<Renderer>().material.color = Color.gray;
        isDash = false;
        SetWayPoint();
        isWaiting = false;
        yield return new WaitForSeconds(4.0f);
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        isCheckList = true;

    }
}
