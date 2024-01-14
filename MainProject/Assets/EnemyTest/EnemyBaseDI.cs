using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseDI : MonoBehaviour
{
    [SerializeField] protected float speed = 3f;
    [SerializeField] protected float dashSpeed = 10f;
    [SerializeField] protected float rotationSpeed = 5f;
    
    protected Transform playerTransform;
    protected Vector3 direction;
    protected Transform rangeA;
    protected Transform rangeB;
    protected LayerMask collisionLayer;
    protected EnemyManagement enemyManagement;
    protected EnemyCollision collision;
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
        if (isDash)
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);
        else
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
