using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
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

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool grappling;

    public bool isEnemy;

    private void Start()
    {
        isEnemy = false;
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // if (!UIMgr.Instance.isFire)
        // {
        if (Input.GetKeyDown(grappleKey)) StartGrapple();

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
        // }

    }

    private void LateUpdate()
    {
        // if (grappling)
        // lr.SetPosition(0, gunTip.position);
    }

    private void StartGrapple()
    {

        if (grapplingCdTimer > 0) return;

        grappling = true;

        pm.freeze = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                isEnemy = true;
                Debug.Log("Enemy");

                StopFie(hit.collider.gameObject);

            }
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);

        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        // lr.enabled = true;
        // lr.SetPosition(0, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;
        if (!isEnemy)
        {
            pm.JumpToPosition(grapplePoint, highestPointOnArc);

            Invoke(nameof(StopGrapple), 1f);
        }

    }

    public void StopGrapple()
    {
        pm.freeze = false;

        grappling = false;

        grapplingCdTimer = grapplingCd;

        //lr.enabled = false;
    }

    /// <param name="go"></param>
    public void StopFie(GameObject go)
    {

        //UIMgr.Instance.isFire = false;
        go.GetComponent<EnemyMgr>().ChangeScale();

        StartCoroutine(DestoryLine());
        //grappling = false;

        //grapplingCdTimer = grapplingCd;
    }
    IEnumerator DestoryLine()
    {
        yield return new WaitForSeconds(1f);

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
