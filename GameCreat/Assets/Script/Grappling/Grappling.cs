using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMove pm;
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

    private bool grappling;

    public bool isEnemy;

    private void Start()
    {
        isEnemy = false;
        pm = GetComponent<PlayerMove>();
    }

    public void PlayerShot()
    {
        if (!UIMgr.Instance.isFire)
        {
            StartGrapple();  

            if (grapplingCdTimer > 0)
                grapplingCdTimer -= Time.deltaTime;
        }
    }


    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        grappling = true;

        pm.freeze = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            if (hit.collider.gameObject.tag== "Enemy")
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
    }

    /// <param name="go"></param>
    public void StopFie(GameObject go)
    {
        StartCoroutine(MoveAndShrinkEnemy(go, gunTip.position, scale));      
    }
    IEnumerator MoveAndShrinkEnemy(GameObject enemy, Vector3 targetPosition, float finalScale)
    {
        yield return new WaitForSeconds(0.5f);

        float speed = 13f; 
        float scaleSpeed = 2f; 

        Vector3 originalScale = enemy.transform.localScale; 

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

        UIMgr.Instance.ChangeBulletNum(1, enemy);

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
