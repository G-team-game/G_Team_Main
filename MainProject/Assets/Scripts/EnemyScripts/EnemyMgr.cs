using UnityEngine;
public class EnemyMgr : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveValue;
    public float atktimer = 0;
    public int atkDamage = 5;
    public bool isFire = false;
    private Vector3 startPos;
    [SerializeField] float impulse = 300;
    bool isCollision = false;

    Rigidbody rigidBody;
    Rigidbody playerRigidBody;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        rigidBody = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidBody = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        atktimer -= Time.deltaTime;

        var x = Mathf.PingPong(Time.time * moveSpeed, moveValue);
        transform.position = new Vector3(transform.position.x, startPos.y + x, transform.position.z);
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