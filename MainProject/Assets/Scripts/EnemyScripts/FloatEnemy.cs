using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEnemy : EnemyBase
{
    [SerializeField] float floatSpeed = 1.0f; // ïÇìÆÇÃë¨Ç≥
    [SerializeField] float floatDistance = 1.0f; // è„â∫Ç…ïÇÇ≠ãóó£
    private Vector3 initialPosition;
    // Start is called before the first frame update
    protected override void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatDistance;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
