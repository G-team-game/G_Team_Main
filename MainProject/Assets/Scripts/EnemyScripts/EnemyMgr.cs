using UnityEngine;
using DG.Tweening;
public class EnemyMgr : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveValue;
    [SerializeField] private Collider collider;
    [SerializeField] private GameObject blackBody, normalBody;
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

    public void Blow()
    {
        FloatEnemy floatEnemy = GetComponent<FloatEnemy>();
        if (floatEnemy != null)
        {
            floatEnemy.enabled = false;
        }

        blackBody.transform.DOShakePosition(0.5f, 1, 50);

        blackBody.SetActive(true);
        normalBody.SetActive(false);

        Vector3 playerVelocity = playerRigidBody.velocity;
        Vector3 forceDirection = playerVelocity.normalized + Vector3.up * 0.5f;
        rigidBody.AddForce(forceDirection * impulse, ForceMode.Impulse);
        GetComponent<Rigidbody>().useGravity = true;
        isCollision = true;
        collider.enabled = false;

        Destroy(gameObject, 5);
    }
}