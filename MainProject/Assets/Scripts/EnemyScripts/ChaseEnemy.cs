//ray�ŏ�Q���������Ă�r��
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : EnemyBase
{
    //player����ǂ��܂ł͈͓̔��Ń����_�����W�����肷�邩
    [SerializeField] float chasingMaxDistance = 15f;
    //player����ǂ̋����ɓ������璼�Œǂ�������悤�ɂ��邩
    [SerializeField] float chasingPlayerDistance = 3f;
    //Player�Ƃǂ̋����ȏ�ɂȂ�������W���Đݒ肷�邩
    [SerializeField] float maxPlayerDistance = 15f;
    private Vector3 chasePosition;
    protected override void Start()
    {
        base.Start();
        GetRandomPositionNearPlayer();
    }
    protected override void Update()
    {
        if(Physics.Raycast(transform.position,chasePosition, out RaycastHit hit, collisionLayer))
        {
            Debug.Log("��Q�������m");
            GetRandomPositionNearPlayer();
        }
        if (Vector3.Distance(transform.position, chasePosition) < 1f||Vector3.Distance(transform.position,playerTransform.position)>maxPlayerDistance)
        {
            GetRandomPositionNearPlayer();
        }

        if (Vector3.Distance(transform.position, playerTransform.position) < chasingPlayerDistance)
        {
            direction = (playerTransform.position - transform.position).normalized;
            GetRandomPositionNearPlayer();
        }
        else
        {
            direction = (chasePosition - transform.position).normalized;
        }
        Debug.DrawRay(transform.position,direction);
        base.Update();
    }

    private void GetRandomPositionNearPlayer()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 randomOffset = Random.insideUnitSphere * chasingMaxDistance;
        Vector3 randomPosition = playerPosition + randomOffset;
        //if (randomposition.y < 0)
        //{
        //    //�n�ʂɖ��܂�Ȃ��悤�ɍ��W�𐳂ɂ��Ă����Ǐ��������ς����ق�����������
        //    randomposition.y = Mathf.Abs(randomposition.y);
        //}
        Collider[] colliders = Physics.OverlapSphere(randomPosition, chasingMaxDistance, collisionLayer);
        if (colliders.Length > 0|| Physics.Linecast(transform.position, randomPosition, out RaycastHit hit, collisionLayer))
        {
            Debug.Log("��Q�������m");
            GetRandomPositionNearPlayer();//������񃉃��_������
        }
        chasePosition = randomPosition;
    }
}
