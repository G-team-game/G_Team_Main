using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderEnemy : EnemyBase
{
    bool isForwardDashing = false;
    bool isCheckList = true;
    bool isCoroutine = false;
    bool isStop=false;
    private Vector3 wanderPosition;
    private Vector3 dashPosition;
    [SerializeField] protected float wanderingDistance = 10f;

    [SerializeField] float rotationTime = 2.0f;
    [SerializeField] float dashTime = 5.0f;
    protected override void Start()
    {
        base.Start();
        wanderPosition= SetWayPoint();
    }
    protected override void Update()
    {
        //Vector3 rayDirection = wanderPosition - transform.position;
        //Ray ray = new Ray(transform.position, rayDirection);
        //RaycastHit hit;
        //if(Physics.Raycast(ray,out hit,Vector3.Distance(transform.position,wanderPosition)))
        //{
        //    Debug.DrawRay(transform.position, rayDirection, Color.red);
        //    Debug.Log("��Q�������o����܂����B");
        //}
        if (isDash && !isCoroutine)
        {
            isCoroutine = true;
            isCheckList = false;
            StartCoroutine(dashCoroutine());
        }
        else if(!isDash)
        {
            if (CheckDashList(this.gameObject) && isCheckList)
            {
                StartDashTowardsPlayer();
            }
            if (Vector3.Distance(transform.position, wanderPosition) < 1.0f)
            {
                wanderPosition= SetWayPoint();
            }
            direction = (wanderPosition - transform.position).normalized;
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
    IEnumerator dashCoroutine()
    {
        Quaternion startRotation=transform.rotation;
        float timeCount = 0.0f;
        while(timeCount<rotationTime)//���̏�Ŏ~�܂���player�����E�ɓ��ꂽ���̍��W�̕�����
        {
            timeCount+=Time.deltaTime;
            float t = Mathf.Clamp01(timeCount / rotationTime);
            Quaternion targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        timeCount = 0.0f;
        while(timeCount<dashTime)//�˂�����ł���
        {
            timeCount+=Time.deltaTime;
            transform.Translate(Vector3.forward*dashSpeed*Time.deltaTime);
            yield return null;

            if(CheckCollision())
            {
                //�ːi�������ɂԂ������Ƃ��̏����Ƃ�
                isStop = true;
                break;
            }
        }
        gameObject.GetComponent<Renderer>().material.color = Color.gray;//�ːi�I�������̑ҋ@���ԓI�Ȃ�
        isDash = false;
        SetWayPoint();
        yield return new WaitForSeconds(2.0f);
        enemyManagement.dashList.Remove(this.gameObject);//���X�g��������Č��̏�Ԝp�j�ɖ߂�
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        isCoroutine = false;
        isCheckList = true;

    }

    private Vector3 SetWayPoint()//�p�j����Ƃ��̈ʒu����
    {
        Vector3 randomPosition;
        while (true)
        {
            randomPosition = GenerateRandomPosition();
            Vector3 rayDirection = randomPosition - transform.position;
            Ray ray = new Ray(transform.position, rayDirection);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, Vector3.Distance(transform.position, randomPosition)))
            {
                Debug.DrawRay(transform.position, rayDirection, Color.red);
                Debug.Log("��Q�������o����܂����B");
                break;
            }
        }
        return randomPosition;

    }
    private Vector3 GenerateRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderingDistance;
        randomDirection.y = Mathf.Abs(randomDirection.y);
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

    private bool CheckCollision()
    {
        //�ːi���Ă�Ƃ��ɉ����ɂԂ�������`���Ă�������p(�܂��������ĂȂ�)
        return false;
    }

}
