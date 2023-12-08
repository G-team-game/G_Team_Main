using System.Collections;
using UnityEngine;
using System.Linq;
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] float chasingMaxDistance = 15f;
    [SerializeField] float playerChaseDistance = 3f;

    private Transform player;
    private Vector3 targetPosition;
    private Vector3 dashTarget;
    private bool isChasing = false;
    private bool isDashing = false;
    private Transform rangeA;
    private Transform rangeB;

    private LayerMask collisionLayer;
    private Vector3 ChasingRandomPosition;

    public EnemyManagement enemyManagement;
    private EnemyBase enemyBase;

    private void Start()
    {
        rangeA = GameObject.Find("ChaseRangeA").transform;
        rangeB = GameObject.Find("ChaseRangeB").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        enemyBase = GetComponent<EnemyBase>();

        enemyManagement = EnemyManagement.Instance;
        if (enemyManagement == null)
        {
            Debug.LogError("EnemyManager not found.");
        }

        // if (!isChasing)
        //     SetWayPoint();
        // else if (isChasing)
        //     ChasingRandomPosition = GetRandomPositionNearPlayer();
    }

    // private void Update()
    // {
    //     Vector3 direction;
    //     if (isDashing)
    //     {
    //         EnemyDash();
    //         if (!CheckDashList())
    //         {
    //             isDashing = false;
    //         }
    //     }
    //     else
    //     {
    //         if (isChasing)
    //         {
    //             if (Vector3.Distance(transform.position, ChasingRandomPosition) < 1f)
    //             {
    //                 ChasingRandomPosition = GetRandomPositionNearPlayer();
    //             }

    //             if (Vector3.Distance(transform.position, player.position) < playerChaseDistance)
    //             {
    //                 direction = (player.position - transform.position).normalized;
    //             }
    //             else
    //             {
    //                 direction = (ChasingRandomPosition - transform.position).normalized;
    //             }
    //             Quaternion targetRotation = Quaternion.LookRotation(direction);
    //             transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);


    //             transform.Translate(Vector3.forward * speed * Time.deltaTime);
    //         }
    //         else if (!isChasing)
    //         {
    //             if (CheckDashList())
    //             {
    //                 StartDashTowardsPlayer();
    //             }

    //             else if (Vector3.Distance(transform.position, targetPosition) < 1f)
    //             {
    //                 SetWayPoint();
    //             }
    //             direction = (targetPosition - transform.position).normalized;
    //             Quaternion targetRotation = Quaternion.LookRotation(direction);
    //             transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    //             transform.Translate(Vector3.forward * speed * Time.deltaTime);

    //         }
    //     }
    // }

    // private Vector3 GetRandomPositionNearPlayer()
    // {
    //     Vector3 playerPosition = player.position;
    //     Vector3 randomOffset = Random.insideUnitSphere * chasingMaxDistance;
    //     Vector3 randomposition = playerPosition + randomOffset;
    //     if (randomposition.y < 0)
    //     {
    //         randomposition.y = Mathf.Abs(randomposition.y);//�n�ʂ�艺�ɍs�������Ȃ��悤��y���W�𐳂�
    //     }
    //     Collider[] colliders = Physics.OverlapSphere(randomposition, chasingMaxDistance, collisionLayer);
    //     if (colliders.Length > 0)
    //     {
    //         return GetRandomPositionNearPlayer();//������񃉃��_������
    //     }
    //     return randomposition;
    // }

    // private void SetWayPoint()
    // {
    //     Vector3 randomPosition = GenerateRandomPosition();
    //     Debug.Log("Random Position: " + randomPosition + this.gameObject.name);//�����_�����W�m�F�p
    //     targetPosition = randomPosition;
    // }

    // private Vector3 GenerateRandomPosition()
    // {
    //     Vector3 randomDirection = Random.insideUnitSphere * wanderingDistance;
    //     randomDirection += transform.position; // ���݂̈ʒu���N�_�ɂ���
    //     randomDirection.y = Mathf.Abs(randomDirection.y); //�n�ʂɖ��܂����Ⴄ�̂�y���W�𐳂�

    //     // �m�F����͈͂̍ŏ��l�ƍő�l�����߂�
    //     Vector3 minRange = new Vector3(
    //         Mathf.Min(rangeA.position.x, rangeB.position.x),
    //         Mathf.Min(rangeA.position.y, rangeB.position.y),
    //         Mathf.Min(rangeA.position.z, rangeB.position.z)
    //     );

    //     Vector3 maxRange = new Vector3(
    //         Mathf.Max(rangeA.position.x, rangeB.position.x),
    //         Mathf.Max(rangeA.position.y, rangeB.position.y),
    //         Mathf.Max(rangeA.position.z, rangeB.position.z)
    //     );

    //     // �͈͓��Ƀ����_���ȍ��W�����邩�m�F����
    //     if (randomDirection.x >= minRange.x && randomDirection.x <= maxRange.x &&
    //         randomDirection.y >= minRange.y && randomDirection.y <= maxRange.y &&
    //         randomDirection.z >= minRange.z && randomDirection.z <= maxRange.z)
    //     {
    //         return randomDirection; // �͈͓��̏ꍇ�͂��̂܂ܕԂ�
    //     }
    //     else
    //     {
    //         // �͈͊O�̏ꍇ�͍ēx��������
    //         return GenerateRandomPosition();
    //     }
    // }

    // private bool CheckDashList()
    // {
    //     return enemyBase.getEnemyType == EnemyType.dash;
    // }

    // private void StartDashTowardsPlayer()
    // {
    //     isDashing = true;
    //     dashTarget = player.position;
    //     Debug.Log("�_�b�V���ړI�n" + dashTarget + this.gameObject.name);
    // }

    // private void EnemyDash()
    // {
    //     Vector3 direction = (dashTarget - transform.position).normalized;
    //     Quaternion targetRotation = Quaternion.LookRotation(direction);
    //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    //     transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);

    //     if (Vector3.Distance(transform.position, dashTarget) < 1.0f)
    //     {
    //         isDashing = false;
    //         //enemyManagement.dashList.Remove(this.gameObject);
    //         enemyBase._enemyState = EnemyState.wander;
    //     }
    // }


    // public void EnemyChaseSetActive()//EnemySpawn������Ăяo���ĒǐՃ��[�htrue�ɂ����[���Ă�ł��B
    // {
    //     isChasing = true;
    //     //Debug.Log("Active!" + isChasing);
    // }
}
