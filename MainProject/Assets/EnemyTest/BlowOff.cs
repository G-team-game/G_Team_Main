using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowOff : MonoBehaviour
{
    bool isCollision = false;
    private Collider enemyCollider;
    private Rigidbody enemyRigidbody;
    FloatEnemy floatEnemy;
    WanderEnemy wanderEnemy;
    ChaseEnemy chaseEnemy;

    [SerializeField] float upwardForce = 10f;
    [SerializeField] float forwardForce=5f;
    [SerializeField] float hiddenObjectTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        floatEnemy = GetComponent<FloatEnemy>();
        wanderEnemy = GetComponent<WanderEnemy>();
        chaseEnemy = GetComponent<ChaseEnemy>();
        enemyCollider = GetComponent<Collider>();
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // �v���C���[�ɓ��������ꍇ
        {
            HandleCollision();
        }
    }

    void HandleCollision()
    {
        //�X�N���v�g�~�߂�
        if (floatEnemy != null)
            floatEnemy.enabled = false;
        if (wanderEnemy != null)
            wanderEnemy.enabled = false;
        if(chaseEnemy != null)
            chaseEnemy.enabled = false;

        // �����蔻��𖳌���
        enemyCollider.enabled = false;

        // Rigidbody��useGravity��L���ɂ���
        enemyRigidbody.useGravity = true;

        // ��������O�����ɗ͂������Đ�����΂�
        Vector3 upwardDirection = Vector3.up * upwardForce;
        Vector3 forwardDirection = transform.forward * forwardForce;
        enemyRigidbody.AddForce(upwardDirection + forwardDirection, ForceMode.Impulse);

        // �I�u�W�F�N�g���\���ɂ��邽�߂̃R���[�`�����J�n
        StartCoroutine(HideObjectAfterDelay(hiddenObjectTime));
    }

    IEnumerator HideObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // �I�u�W�F�N�g���\���ɂ���
        gameObject.SetActive(false);
    }
}
