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
        if (collision.gameObject.CompareTag("Player")) // プレイヤーに当たった場合
        {
            HandleCollision();
        }
    }

    void HandleCollision()
    {
        //スクリプト止める
        if (floatEnemy != null)
            floatEnemy.enabled = false;
        if (wanderEnemy != null)
            wanderEnemy.enabled = false;
        if(chaseEnemy != null)
            chaseEnemy.enabled = false;

        // 当たり判定を無効化
        enemyCollider.enabled = false;

        // RigidbodyのuseGravityを有効にする
        enemyRigidbody.useGravity = true;

        // 上方向かつ前方向に力を加えて吹っ飛ばす
        Vector3 upwardDirection = Vector3.up * upwardForce;
        Vector3 forwardDirection = transform.forward * forwardForce;
        enemyRigidbody.AddForce(upwardDirection + forwardDirection, ForceMode.Impulse);

        // オブジェクトを非表示にするためのコルーチンを開始
        StartCoroutine(HideObjectAfterDelay(hiddenObjectTime));
    }

    IEnumerator HideObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // オブジェクトを非表示にする
        gameObject.SetActive(false);
    }
}
