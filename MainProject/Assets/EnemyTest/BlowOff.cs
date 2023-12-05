using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowOff : MonoBehaviour
{
    [SerializeField] float impulse = 300;
    bool isCollision = false;

    Rigidbody rigidBody;
    Rigidbody playerRigidBody;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        //プレイヤーをタグで検索し、Rigidbodyを取得
        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidBody = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && isCollision == false)
        {
            //吹っ飛ばす
            FloatEnemy floatEnemy=GetComponent<FloatEnemy>();
            if (floatEnemy != null)
            {
                floatEnemy.enabled = false;
            }
            Vector3 playerVelocity = playerRigidBody.velocity;
            //rigidBody.AddForce(playerVelocity * impulse, ForceMode.Impulse);
            //rigidBody.AddForce(Vector3.up * impulse, ForceMode.Impulse);
            Vector3 forceDirection = playerVelocity.normalized + Vector3.up * 0.5f; // ここで0.5fは調整可能な係数
            rigidBody.AddForce(forceDirection * impulse, ForceMode.Impulse);
            GetComponent<Rigidbody>().useGravity = true;
            isCollision = true;
        }
    }
}
