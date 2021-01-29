using Assets.Scripts;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleLook : MonoBehaviour
{
    enum GrappleState
    {
        Idle,
        Shot,
        Attached
    }

    private bool mIsGrappling;
    private Vector3 mScreenCenter;

    private GameObject mMarkedObject;
    private Vector3 mAttachedPosition;

    private GrappleState mGrappleState;

    private Quaternion mHandR_defaultRotation;
    private Quaternion mHandL_defaultRotation;

    [SerializeField] Transform mHandR;
    [SerializeField] Transform mHandL;

    [SerializeField] Transform mPlayerBody;
    [SerializeField] LineRenderer mLineRenderer;

    private void Start()
    {
        DOTween.Init();
        mIsGrappling = false;
        mScreenCenter =  new Vector3(Screen.width / 2, Screen.height / 2, 0); 
        mMarkedObject = null;
        mGrappleState = GrappleState.Idle;
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
            GrappableObject grappleObject = hit.collider.gameObject.GetComponent<GrappableObject>();
            if (grappleObject)
            {
                
                //if the collided object is not grappable return
                //get landing zone for hit 
                MeshFilter meshFilter = hit.collider.gameObject.GetComponent<MeshFilter>();

                float extent = meshFilter.mesh.bounds.extents.y;
                float scale = hit.collider.transform.localScale.y;

                Vector3 grabPosition = hit.transform.position + new Vector3( 0, extent * scale - 2, 0);
                Vector3 landingPosition = grabPosition + Vector3.up * 4;

                //Calculate object distance to decide of grapple is possible
                float dist = Vector3.Distance(landingPosition, mPlayerBody.position);

                if (dist < 25)
                {
                    mMarkedObject = hit.collider.gameObject;

                    //Mark the object as grappable
                    mMarkedObject.GetComponent<Renderer>().material.color = Color.green;

                    //If left mouse button is pressed Initiate grapple
                    if (Input.GetMouseButtonDown(0))
                    {
                        mGrappleState = GrappleState.Shot;

                        mHandR_defaultRotation = mHandR.rotation;
                        mHandL_defaultRotation = mHandL.rotation;

                        RotateHandToPoint(mHandR, grabPosition);
                        RotateHandToPoint(mHandL, grabPosition);

                        mHandL.DOScaleX(dist, .5f);
                        mHandR.DOScaleX(dist, .5f).OnComplete(() =>
                        {
                            mIsGrappling = true;

                            mGrappleState = GrappleState.Attached;
                            mAttachedPosition = grabPosition;

                            //Get grapping type
                            GrappleType type = grappleObject.GetGrappleType();

                            //Handle different types of grapple
                            switch (type)
                            {
                                case GrappleType.None:
                                    mHandL.DOScaleX(1, .5f);
                                    mHandR.DOScaleX(1, .5f).OnComplete(() =>
                                    {
                                        FinishGrappling();
                                    });
                                    break;
                                case GrappleType.Heavy:
                                    //dotween to the wanted position
                                    ResetHandsScale();

                                    mPlayerBody.DOMove(landingPosition, .5f).OnComplete(() =>
                                    {
                                        FinishGrappling();
                                    });
                                    break;

                                case GrappleType.Light:
                                    //Snap the object to the player
                                    ResetHandsScale();

                                    var colliderTransform = hit.collider.gameObject.transform;
                                    var dest = mPlayerBody.position + mPlayerBody.forward * 2;
                                    colliderTransform.DOMove(new Vector3(dest.x, colliderTransform.position.y, dest.z), .5f)
                                    .OnComplete(() =>
                                    {
                                        FinishGrappling();
                                    });
                                    break;

                                case GrappleType.VeryLight:
                                    //Snap the object to the player
                                    ResetHandsScale();

                                    hit.collider.gameObject.GetComponent<Rigidbody>().velocity = UnityEngine.Random.onUnitSphere * 20;

                                    FinishGrappling();
                                    break;
                            }
                        });

                        

                        

                        
                    }
                }
                else
                {
                    //If the object is too far away we would like to mark it as red
                    mMarkedObject = hit.collider.gameObject;

                    //Mark the object as grappable
                    mMarkedObject.GetComponent<Renderer>().material.color = Color.red;
                }

                
            }
                
        }
        
    }

    private void ResetHandsScale()
    {
        mHandR.DOScaleX(1, .5f);
        mHandL.DOScaleX(1, .5f);
    }

    private void FinishGrappling()
    {
        mIsGrappling = false; ;
        mHandR.rotation = mHandR_defaultRotation;
        mHandL.rotation = mHandL_defaultRotation;
    }

    private void RotateHandToPoint(Transform hand, Vector3 position)
    {
        Vector3 vec = hand.position - position;

        hand.Rotate(0, Vector3.Angle(-vec, transform.forward), 0, Space.Self);
    }
}