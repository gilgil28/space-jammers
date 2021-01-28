using Assets.Scripts;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleLook : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public Transform playerBody;
    public LineRenderer lineRenderer;
    private bool isGrappling;
    private Vector3 screenCenter;
    private void Start()
    {
        DOTween.Init();
        isGrappling = false;
        screenCenter =  new Vector3(Screen.width / 2, Screen.height / 2, 0); ;
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out hit) && !isGrappling)
        {
            if (Input.GetMouseButton(0))
            {
                Debug.Log(hit.collider.name);

                //if the collided object is not grappable return
                GrappableObject grab = hit.collider.gameObject.GetComponent<GrappableObject>();
                if (grab)
                {
                    //get landing zone for hit 
                    MeshFilter meshFilter = hit.collider.gameObject.GetComponent<MeshFilter>();

                    float extent = meshFilter.mesh.bounds.extents.y;
                    float scale = hit.collider.transform.localScale.y;

                    Vector3 landingPosition = hit.transform.position + new Vector3( 0, extent * scale + 2, 0);

                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, playerBody.position);
                    lineRenderer.SetPosition(1, hit.transform.position);

                    //dotween to the wanted position
                    playerBody.DOMove(landingPosition, .5f).OnComplete(() =>
                    {
                        isGrappling = false; ;
                        lineRenderer.enabled = false;
                    });
                    //LeanTween.move(playerBody.gameObject, landingPosition, 1);

                    //StartCoroutine(MoveInCurve(hit.transform));
                    isGrappling = true;
                }

                
            }
                
        }
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, playerBody.position);
        }
        
    }

    IEnumerator MoveInCurve(Transform endPoint)
    {
        float BezierTime = 0;



        while (BezierTime < 1)
        {
            
            transform.position = GetBezierPosition(playerBody, endPoint, BezierTime);
            BezierTime = BezierTime + Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //while (BezierTime < 1) {
        //    float CurveX = (((1 - BezierTime) * (1 - BezierTime)) * startPoint.x) + (2 * BezierTime * (1 - BezierTime) * controlPoint.x) + ((BezierTime * BezierTime) * endPoint.x);
        //    float CurveY = (((1 - BezierTime) * (1 - BezierTime)) * startPoint.y) + (2 * BezierTime * (1 - BezierTime) * controlPoint.y) + ((BezierTime * BezierTime) * endPoint.y);
        //    float CurveZ = (((1 - BezierTime) * (1 - BezierTime)) * startPoint.z) + (2 * BezierTime * (1 - BezierTime) * controlPoint.z) + ((BezierTime * BezierTime) * endPoint.z);
        //    transform.position = new Vector3(CurveX, CurveY, CurveZ);
        //    BezierTime = BezierTime + Time.deltaTime;
        //    yield return new WaitForEndOfFrame();
        //}
        isGrappling = false;
    }

    // parameter t ranges from 0f to 1f
    // this code might not compile!
    Vector3 GetBezierPosition(Transform transformBegin, Transform transformEnd, float t)
    {
        Vector3 p0 = transformBegin.position;
        Vector3 p1 = p0 + transformBegin.forward;
        Vector3 p3 = transformEnd.position;
        Vector3 p2 = p3 - transformEnd.forward;

        // here is where the magic happens!
        return Mathf.Pow(1f - t, 3f) * p0 + 3f * Mathf.Pow(1f - t, 2f) * t * p1 + 3f * (1f - t) * Mathf.Pow(t, 2f) * p2 + Mathf.Pow(t, 3f) * p3;
    }
}