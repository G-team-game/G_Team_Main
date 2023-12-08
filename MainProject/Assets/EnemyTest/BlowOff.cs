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

        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidBody = player.GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && isCollision == false)
        {
            FloatEnemy floatEnemy = GetComponent<FloatEnemy>();
            if (floatEnemy != null)
            {
                floatEnemy.enabled = false;
            }
            Vector3 playerVelocity = playerRigidBody.velocity;
            Vector3 forceDirection = playerVelocity.normalized + Vector3.up * 0.5f;
            rigidBody.AddForce(forceDirection * impulse, ForceMode.Impulse);
            GetComponent<Rigidbody>().useGravity = true;
            isCollision = true;
        }
    }
}