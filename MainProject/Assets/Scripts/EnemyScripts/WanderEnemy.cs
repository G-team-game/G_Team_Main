//���͂��̂܂ܓ˂����ނ��ǕǂƂ��ɂԂ�������~�܂�悤�ɂ���H
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderEnemy : EnemyBase
{
    bool isForwardDashing = false;
    bool isCheckList = true;
    private Vector3 wanderPosition;
    private Vector3 dashPosition;
    //�����_�����W�����肷��Ƃ���enemy����ǂ��܂ł͈̔͂ō��W�����߂邩
    [SerializeField] protected float wanderingDistance = 30f;
    protected override void Start()
    {
        base.Start();
        SetWayPoint();
    }
    protected override void Update()
    {
        if (isForwardDashing)
        {
            transform.Translate(Vector3.forward*dashSpeed*Time.deltaTime);
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
        isForwardDashing = true;
        isCheckList = false;
        enemyManagement.dashList.Remove(this.gameObject);
        gameObject.GetComponent<Renderer>().material.color = Color.black;
        //Collider�łԂ���܂Ł`�݂����Ȋ����ɂ��悤�Ƃ������Ǖǂň͂܂�Ă�킯����Ȃ����痈�T�Ԃ���or��莞�Ԍo�߂ŃX�g�b�v�݂����Ȃ̂ɂ���
        yield return new WaitForSeconds(3.0f);
        gameObject.GetComponent<Renderer>().material.color = Color.gray;
        isDash = false;
        SetWayPoint();
        isForwardDashing = false;
        yield return new WaitForSeconds(2.0f);
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        isCheckList = true;

    }

    protected virtual void SetWayPoint()
    {
        Vector3 randomPosition = GenerateRandomPosition();
        wanderPosition = randomPosition;

    }
    private Vector3 GenerateRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderingDistance;
        randomDirection += transform.position; // ���݂̈ʒu���N�_�ɂ���
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


}
