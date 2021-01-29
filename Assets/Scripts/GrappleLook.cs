using Assets.Scripts;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleLook : MonoBehaviour
{
    private bool mIsGrappling;
    private Vector3 mScreenCenter;

    private GameObject mMarkedObject;

    [SerializeField] Transform mPlayerBody;
    [SerializeField] LineRenderer mLineRenderer;

    private void Start()
    {
        DOTween.Init();
        mIsGrappling = false;
        mScreenCenter =  new Vector3(Screen.width / 2, Screen.height / 2, 0); ;
        mMarkedObject = null;
    }

    void Update()
    {
        //Reset the last cached markerd object to it's original color
        if (mMarkedObject != null)
        {
            mMarkedObject.GetComponent<Renderer>().material.color = Color.white;
            mMarkedObject = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(mScreenCenter);
        if (Physics.Raycast(ray, out RaycastHit hit) && !mIsGrappling)
        {
            GrappableObject isGrappable = hit.collider.gameObject.GetComponent<GrappableObject>();
            if (isGrappable)
            {
                

                Debug.Log(hit.collider.name);

                //if the collided object is not grappable return
                //get landing zone for hit 
                MeshFilter meshFilter = hit.collider.gameObject.GetComponent<MeshFilter>();

                float extent = meshFilter.mesh.bounds.extents.y;
                float scale = hit.collider.transform.localScale.y;

                Vector3 grabPosition = hit.transform.position + new Vector3( 0, extent * scale, 0);
                Vector3 landingPosition = grabPosition + Vector3.up * 2;

                //Calculate object distance to decide of grapple is possible
                float dist = Vector3.Distance(landingPosition, mPlayerBody.position);

                if (dist < 25)
                {
                    mMarkedObject = hit.collider.gameObject;

                    //Mark the object as grappable
                    mMarkedObject.GetComponent<Renderer>().material.color = Color.green;

                    //If left mouse button is pressed Initiate grapple
                    if (Input.GetMouseButton(0))
                    {
                        mLineRenderer.enabled = true;
                        mLineRenderer.SetPosition(0, mPlayerBody.position);
                        mLineRenderer.SetPosition(1, mPlayerBody.position);
                        Vector3 lineEnd = mLineRenderer.GetPosition(1);

                        DOTween.To(() => lineEnd, x => mLineRenderer.SetPosition(1, x), grabPosition, .5f).OnComplete(()=>
                        {
                            //dotween to the wanted position
                            mPlayerBody.DOMove(landingPosition, .5f).OnComplete(() =>
                            {
                                mIsGrappling = false; ;
                                mLineRenderer.enabled = false;
                            });
                        });
                        //DOTween.To(() => myFloat, x => myFloat = x, 52, 1);

                        

                        mIsGrappling = true;
                    }
                }
                else
                {
                    mMarkedObject = hit.collider.gameObject;

                    //Mark the object as grappable
                    mMarkedObject.GetComponent<Renderer>().material.color = Color.red;
                }

                
            }
                
        }
        if (mIsGrappling)
        {
            mLineRenderer.SetPosition(0, mPlayerBody.position);
        }
        
    }
}