using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderEnemy : EnemyBase
{
    bool isRotating = false;
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
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float t = 0;

            while (t < 1)
            {
                t += Time.deltaTime / 1.0f; // 1•b‚©‚¯‚ÄŒü‚«‚ð•Ï‚¦‚é
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
            }
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position,dashPosition) < 1.0f)
            {
                isDash = false;
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
                enemyManagement.dashList.Remove(this.gameObject);
            }
        }
        else
        {
            if (CheckDashList(this.gameObject))
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

    IEnumerator RotationTowardsPlayer()
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float elapsedTime = 0;
        float rotationTime = 1.0f;
        while (elapsedTime < rotationTime)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(MoveTowardsPlayer());
    }
    IEnumerator MoveTowardsPlayer()
    {
        float moveTime = 2.0f;
        float decelerationTime = 1.0f;
        for(float t=0;t<moveTime;t+=Time.deltaTime)
        {
            Vector3 moveDirection=(dashPosition-transform.position).normalized;
            transform.Translate(moveDirection*dashSpeed*Time.deltaTime);

            
        }
        yield return null;
    }

    private bool CheckDashList(GameObject obj)
    {
        return enemyManagement.dashList.Contains(obj);
    }

    private void StartDashTowardsPlayer()
    {
        isDash = true;
        dashPosition = playerTransform.position;
        StartCoroutine(RotationTowardsPlayer());
    }
}
