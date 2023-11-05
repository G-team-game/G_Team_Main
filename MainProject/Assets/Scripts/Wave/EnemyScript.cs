using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //�ːi��spawner�̃��X�g���烉���_���Ŏw�萔���I��ł����銴����
    //�ːi�̎d�g�ݓI�ɂ�
    //�E�v���C���[�̍��W�擾�������Ƀ_�b�V�������W��������X�g�b�v
    //���Ȃ����Ċ����ł��B

    [SerializeField] float speed = 3f;//�X�s�[�h
    [SerializeField] float rotationSpeed = 5f;//����X�s�[�h
    //[SerializeField] float DistanceFromAB = 10f;�g���ĂȂ��̂ň�U����
    [SerializeField] float wanderingDistance = 30f;//�����_���ňړ�����Ƃ���enemy����̋������Ő������邽�߂̂��
    [SerializeField] float ChasingMaxDistance = 15f;//�ǐՏ������Ƀ����_���ō��W�w�肷��Ƃ��̃v���C���[����̋���
    [SerializeField] float playerChaseDistance = 3f;//�v���C���[�𒼂Œǂ������郂�[�h�Ɉڍs����Ƃ��̋��� 

    private Transform player; // �v���C���[�̈ʒu
    private Vector3 targetPosition; // ���݂̖ړI�n
    private bool isChasing = false; // �ǐՃ��[�h���ǂ���
    private Transform rangeA;
    private Transform rangeB;

    private LayerMask collisionLayer; // �Փ˔�����s�����C���[�}�X�N
    private Vector3 ChasingRandomPosition;

    public EnemySpawn enemySpawn;

    private void Start()
    {
        rangeA = GameObject.Find("ChaseRangeA").transform;
        rangeB = GameObject.Find("ChaseRangeB").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemySpawn=EnemySpawn.Instance;
        if (enemySpawn == null)
        {
            Debug.LogError("EnemySpawn not found.");
        }


        if (!isChasing)
            SetWayPoint();
        else if(isChasing)
            ChasingRandomPosition = GetRandomPositionNearPlayer();
    }

    private void Update()
    {
        Vector3 direction;
        if (isChasing)
        {
            if(Vector3.Distance(transform.position,ChasingRandomPosition)<1f)
            {
                ChasingRandomPosition = GetRandomPositionNearPlayer();
            }
            
            //Debug.Log(ChasingRandomPosition);
            if(Vector3.Distance(transform.position,player.position)<playerChaseDistance)
            {
                direction = (player.position - transform.position).normalized;
            }
            else
            {
                direction = (ChasingRandomPosition - transform.position).normalized;
            }
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else if(!isChasing)// �p�j���鏈��
        {
            if(CheckDashList(this.gameObject))
            {
                Debug.Log("�_�b�V����"+this.gameObject.name);
            }
            else if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                SetWayPoint();
            }
            direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            
        }
    }

    private Vector3 GetRandomPositionNearPlayer()
    {
        Vector3 playerPosition = player.position;
        Vector3 randomOffset = Random.insideUnitSphere * ChasingMaxDistance;
        Vector3 randomposition = playerPosition + randomOffset;
        if (randomposition.y < 0)
        {
            randomposition.y = Mathf.Abs(randomposition.y);//�n�ʂ�艺�ɍs�������Ȃ��悤��y���W�𐳂�
        }
        Collider[] colliders = Physics.OverlapSphere(randomposition, ChasingMaxDistance, collisionLayer);
        if(colliders.Length>0)
        {
            return GetRandomPositionNearPlayer();//������񃉃��_������
        }
        return randomposition;
    }
    private void SetWayPoint()
    {
        Vector3 randomPosition = GenerateRandomPosition();
        Debug.Log("Random Position: " + randomPosition+this.gameObject.name);//�����_�����W�m�F�p
        targetPosition = randomPosition;

    }

    private Vector3 GenerateRandomPosition()
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
    private bool CheckDashList(GameObject obj)
    {
        return enemySpawn.DashList.Contains(obj);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            //Debug.Log("Player�ɓ�������!");
        }
    }

    public void EnemyChaseSetActive()//EnemySpawn������Ăяo���ĒǐՃ��[�htrue�ɂ����[���Ă�ł��B
    {
        isChasing = true;
        //Debug.Log("Active!" + isChasing);
    }
}
