using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float speed = 3f;
    [SerializeField] protected float rotationSpeed = 5f;
    [SerializeField] protected float dashSpeed = 10f;
    [SerializeField] protected float wanderingDistance = 30f;
    [SerializeField] protected float chasingMaxDistance = 15f;
    [SerializeField] protected float chasingPlayerDistance = 3f;

    protected Transform playerTransform;
    protected Vector3 wanderPosition;
    protected Vector3 chasePosition;
    protected Vector3 dashPosition;
    protected Vector3 direction;
    protected Transform rangeA;
    protected Transform rangeB;

    protected LayerMask collisionLayer;
    protected EnemyManagement enemyManagement;

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
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected virtual void SetWayPoint()//�p�j
    {
        Vector3 randomPosition = GenerateRandomPosition();
        //Debug.Log("Random Position: " + randomPosition + this.gameObject.name);
        wanderPosition = randomPosition;

    }
    private Vector3 GenerateRandomPosition()//�p�j
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

        //���͉���enemy���炠����x�̋���(wanderingDistance)�̊ԂŃ����_���ŖړI�n�ݒ肵�Ă��܂��B
        //��ŃR�����g�A�E�g���Ă�̂��X�e�[�W���rangeA��rangeB�̊ԂŊ��S�����_���Ō��܂���ł��B
        //�ꉞ�ǂ������g����������Ȃ��̂Ŏc���Ă܂��B
        Vector3 randomDirection = Random.insideUnitSphere * wanderingDistance;
        randomDirection += transform.position; // ���݂̈ʒu���N�_�ɂ���
        randomDirection.y = Mathf.Abs(randomDirection.y); //�n�ʂɖ��܂����Ⴄ�̂�y���W�𐳂�

        // �m�F����͈͂̍ŏ��l�ƍő�l�����߂�
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

        // �͈͓��Ƀ����_���ȍ��W�����邩�m�F����
        if (randomDirection.x >= minRange.x && randomDirection.x <= maxRange.x &&
            randomDirection.y >= minRange.y && randomDirection.y <= maxRange.y &&
            randomDirection.z >= minRange.z && randomDirection.z <= maxRange.z)
        {
            return randomDirection; // �͈͓��̏ꍇ�͂��̂܂ܕԂ�
        }
        else
        {
            // �͈͊O�̏ꍇ�͍ēx��������
            return GenerateRandomPosition();
        }
    }
    protected virtual void GetRandomPositionNearPlayer()//�ǐ�
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 randomOffset = Random.insideUnitSphere * chasingMaxDistance;
        Vector3 randomposition = playerPosition + randomOffset;
        if (randomposition.y < 0)
        {
            randomposition.y = Mathf.Abs(randomposition.y);//�n�ʂ�艺�ɍs�������Ȃ��悤��y���W�𐳂�
        }
        Collider[] colliders = Physics.OverlapSphere(randomposition, chasingMaxDistance, collisionLayer);
        if (colliders.Length > 0)
        {
            GetRandomPositionNearPlayer();//������񃉃��_������
        }
        chasePosition= randomposition;
    }
}
