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
        if (isDash)
        {
            StartCoroutine(RotateMove());
            isDash = false;
            direction = (dashPosition - transform.position).normalized;
            
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

    //コルーチン使ってやる?
    IEnumerator RotateMove()
    {
        Quaternion targetRotation = Quaternion.LookRotation(dashPosition-transform.position);
        float elapsedTime = 0;
        float rotationTime = 1.0f;

        while (elapsedTime < rotationTime)
        {
            Debug.Log("向き変え");
            isRotating = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isRotating = false;
        float moveSpeed = dashSpeed;
        while (Vector3.Distance(transform.position, dashPosition) > 0.01f)
        {
            Debug.Log("移動");
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, dashPosition, dashSpeed * Time.deltaTime);
            yield return null;
        }
        float decelerationTime = 1.0f;
        float initialSpeed = dashSpeed;
        float timer = 0;

        while (timer < decelerationTime)
        {
            Debug.Log("減速");
            moveSpeed  = Mathf.Lerp(initialSpeed, 0, timer / decelerationTime);
            timer += Time.deltaTime;
            transform.Translate(Vector3.forward*moveSpeed*Time.deltaTime);
            yield return null;
        }
        Debug.Log("すとっぷ");
        yield return new WaitForSeconds(1.0f);
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
