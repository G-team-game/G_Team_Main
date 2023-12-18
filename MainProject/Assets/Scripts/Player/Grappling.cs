using System.Collections;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Scale")]
    public float scale;
    [Header("Camera")]
    [SerializeField] private PlayerCam playerCam;

    private bool grappling;

    public bool isEnemy;

    private EnemyMgr targetEnemy;

    private void Start()
    {
        isEnemy = false;
    }

    public RaycastHit PlayerShot()
    {
        playerCam.CameraShake(false);

        grappling = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                isEnemy = true;

                targetEnemy = hit.collider.GetComponent<EnemyMgr>();
            }

            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), 0.1f);
        }

        return hit;
    }

    public bool canGrapple()
    {
        RaycastHit hit;
        Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable);
        return hit.collider;
    }

    private void ExecuteGrapple()
    {
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;
    }

    public void StopGrapple()
    {
        grappling = false;

        grapplingCdTimer = grapplingCd;

        playerCam.CameraShake(true);
    }

    IEnumerator MoveAndShrinkEnemy(EnemyBase enemy, Vector3 targetPosition, float finalScale)
    {
        float speed = 13f;
        float scaleSpeed = 2f;

        Vector3 targetScale = Vector3.one * finalScale;

        while (Vector3.Distance(enemy.transform.position, targetPosition) > 0.1f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPosition, speed * Time.deltaTime);

            enemy.transform.localScale = Vector3.MoveTowards(enemy.transform.localScale, targetScale, scaleSpeed * Time.deltaTime);

            yield return null;
        }

        enemy.transform.SetParent(UIMgr.Instance.gunPos);
        enemy.transform.localPosition = new Vector3(0, 0, 0.1f);
        enemy.transform.localScale = targetScale;

        grappling = false;
        isEnemy = false;
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
