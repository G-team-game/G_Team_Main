using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyState
{
    idle,
    wander,
    tracking,
    restraint,
}
public enum EnemyType
{
    normal,
    tracking,
    dash,
}
public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float speed = 3f;
    [SerializeField] protected float dashSpeed = 10f;
    // [SerializeField] protected float wanderingDistance = 30f;
    [SerializeField] protected float chasingMaxDistance = 15f;
    [SerializeField] protected float chasingPlayerDistance = 3f;
    [SerializeField] private EnemyType enemyType;
    public EnemyType getEnemyType { get { return enemyType; } private set { enemyType = value; } }

    [SerializeField] private EnemyState enemyState;
    public EnemyState _enemyState { get { return enemyState; } set { enemyState = value; } }
    [SerializeField] protected float rotationSpeed = 5f;

    protected Transform playerTransform;
    protected Vector3 direction;
    protected Vector3 wanderPosition, chasePosition;
    protected Transform rangeA;
    protected Transform rangeB;
    protected LayerMask collisionLayer;
    protected EnemyManagement enemyManagement;
    protected EnemyCollision collision;
    protected bool isDash = false;

    protected virtual void Start()
    {
        enemyManagement = EnemyManagement.Instance;
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
        if (enemyState == EnemyState.restraint) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        if (isDash)
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);
        else
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected virtual void SetWayPoint()
    {
        Vector3 randomPosition = GenerateRandomPosition();
        wanderPosition = randomPosition;
    }

    private Vector3 GenerateRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection += transform.position;
        randomDirection.y = Mathf.Abs(randomDirection.y);

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

        if (randomDirection.x >= minRange.x && randomDirection.x <= maxRange.x &&
            randomDirection.y >= minRange.y && randomDirection.y <= maxRange.y &&
            randomDirection.z >= minRange.z && randomDirection.z <= maxRange.z)
        {
            return randomDirection;
        }
        else
        {
            return GenerateRandomPosition();
        }
    }

    protected virtual void GetRandomPositionNearPlayer()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 randomOffset = Random.insideUnitSphere * chasingMaxDistance;
        Vector3 randomposition = playerPosition + randomOffset;
        if (randomposition.y < 0)
        {
            randomposition.y = Mathf.Abs(randomposition.y);
        }
        Collider[] colliders = Physics.OverlapSphere(randomposition, chasingMaxDistance, collisionLayer);
        if (colliders.Length > 0)
        {
            GetRandomPositionNearPlayer();
        }
        // chasePosition = randomposition;
    }
}