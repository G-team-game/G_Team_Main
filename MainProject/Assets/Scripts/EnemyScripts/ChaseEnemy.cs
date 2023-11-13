//�ׂ��������Ƃ����܂����Ⴄ�̂Ƃ��Ȃ�Ƃ�������
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
        if (Vector3.Distance(transform.position, chasePosition) < 1f||Vector3.Distance(transform.position,chasePosition)>maxPlayerDistance)
        {
            GetRandomPositionNearPlayer();
        }

        if (Vector3.Distance(transform.position, playerTransform.position) < chasingPlayerDistance)
        {
            direction = (playerTransform.position - transform.position).normalized;
        }
        else
        {
            direction = (chasePosition - transform.position).normalized;
        }
        base.Update();
    }

    private void GetRandomPositionNearPlayer()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 randomOffset = Random.insideUnitSphere * chasingMaxDistance;
        Vector3 randomposition = playerPosition + randomOffset;
        //if (randomposition.y < 0)
        //{
        //    //�n�ʂɖ��܂�Ȃ��悤�ɍ��W�𐳂ɂ��Ă����Ǐ��������ς����ق�����������
        //    randomposition.y = Mathf.Abs(randomposition.y);
        //}
        Collider[] colliders = Physics.OverlapSphere(randomposition, chasingMaxDistance, collisionLayer);
        if (colliders.Length > 0)
        {
            GetRandomPositionNearPlayer();//������񃉃��_������
        }
        chasePosition = randomposition;
    }
}
