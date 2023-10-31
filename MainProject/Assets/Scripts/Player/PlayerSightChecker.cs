using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSightChecker : MonoBehaviour
{
    private Camera playerCamera;
    [SerializeField] private float maxRaycastDistance = 50f;
    EnemyManagement enemyManager;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera= Camera.main;
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        // �J�����̈ʒu
        Vector3 cameraPosition = playerCamera.transform.position;

        // �J�����̑O���x�N�g��
        Vector3 cameraForward = playerCamera.transform.forward;

        // ����p
        float fieldOfView = playerCamera.fieldOfView;

        // ����͈͂̕��ƍ���
        float halfWidth = Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfHeight = halfWidth / playerCamera.aspect;

        // �����̃��C�L���X�g����ˏ�ɔ�΂�
        for (float x = -1f; x <= 1f; x += 0.2f)
        {
            for (float y = -1f; y <= 1f; y += 0.2f)
            {
                Vector3 rayDirection = cameraForward + playerCamera.transform.right * x * halfWidth + playerCamera.transform.up * y * halfHeight;
                Ray ray = new Ray(cameraPosition, rayDirection);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit,maxRaycastDistance))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (hitObject.CompareTag("WanderEnemy"))
                    {
                        Debug.Log("���m");
                        hitObject.GetComponent<Renderer>().material.color = Color.white;
                        enemyManager.SelectDashEnemy(hitObject);
                    }
                }
            }
        }
    }
}
