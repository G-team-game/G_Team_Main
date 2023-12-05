using UnityEngine;
public class EnemyMgr : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveValue;
    public float atktimer = 0;
    public int atkDamage = 5;
    public bool isFire = false;

    private Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        atktimer -= Time.deltaTime;

        var x = Mathf.PingPong(Time.time * moveSpeed, moveValue);
        transform.position = new Vector3(transform.position.x, startPos.y + x, transform.position.z);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isFire)
        {
            if (atktimer <= 0)
            {
                atktimer = 1;
                other.GetComponent<PlayerHP>().Hit(atkDamage);
            }
        }
    }
}