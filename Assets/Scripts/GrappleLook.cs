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

    enum GrappleOption
    {
        None,
        CanGrapple,
        CannotGrapple
    }

    private const float GRAPPLE_RADIUS_LENGTH = 7f;
    private const float GRAPPLE_MAX_HEIGHT_DIFF = 4;

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
    private float _yAngle;

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
            HighlightObject(mMarkedObject, GrappleOption.None);
            mMarkedObject = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(mScreenCenter);
        if (Physics.Raycast(ray, out RaycastHit hit) && !mIsGrappling)
        {
            GameObject hitCollider = hit.collider.gameObject;
            GrappableObject grappleObject = hitCollider.GetComponent<GrappableObject>();
            if (grappleObject)
            {
                mHandR_defaultRotation = mHandR.rotation;
                mHandL_defaultRotation = mHandL.rotation;
                
                //if the collided object is not grappable return
                //get landing zone for hit 
                MeshFilter meshFilter = hitCollider.GetComponent<MeshFilter>();

                Vector3 topCenter = hit.collider.bounds.center + hit.collider.bounds.extents.y * Vector3.up;
                var grapPosition = hit.point;
                Vector3 landingPosition = topCenter + Vector3.up * 0.15f;

                //If a pre defined landing position was specified make the one that was defined
                Vector3 predefinedLandingPoint = grappleObject.GetPredefinedLandingPoint();
                if (predefinedLandingPoint != Vector3.zero)
                {
                    landingPosition = predefinedLandingPoint;
                }

                //Calculate object distance to decide of grapple is possible
                float dist = Vector3.Distance(grapPosition, mPlayerBody.position);

                float y_diff = Mathf.Abs(landingPosition.y - transform.position.y);

                Debug.Log("y_diff: " + y_diff);
                Debug.Log("dist: " + dist);

                if (dist < GRAPPLE_RADIUS_LENGTH && y_diff < GRAPPLE_MAX_HEIGHT_DIFF && grappleObject.GetGrappleType() != GrappleType.None)
                {
                    mMarkedObject = hitCollider;

                    //Mark the object as grappable
                    HighlightObject(mMarkedObject, GrappleOption.CanGrapple);
                    

                    //If left mouse button is pressed Initiate grapple
                    if (Input.GetMouseButtonDown(0))
                    {
                        mGrappleState = GrappleState.Shot;

                        grappleObject.OnGrapped();

                        RotateHandToPoint(mHandR, grapPosition);
                        RotateHandToPoint(mHandL, grapPosition);

                        mHandL.DOScaleX(dist, .5f);
                        mHandR.DOScaleX(dist, .5f).OnComplete(() =>
                        {
                            mIsGrappling = true;

                            mGrappleState = GrappleState.Attached;
                            mAttachedPosition = topCenter;

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

                                    mPlayerBody.DOMove(landingPosition, .5f).SetEase(Ease.OutQuad).OnComplete(() =>
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
                                case GrappleType.Special:
                                    Animator anim = hitCollider.GetComponent<Animator>();
                                    if (!anim)
                                    {
                                        throw new Exception("Special object should always have animators");
                                    }
                                    anim.SetTrigger("Hit");

                                    mHandL.DOScaleX(1, .5f);
                                    mHandR.DOScaleX(1, .5f).OnComplete(() =>
                                    {
                                        FinishGrappling();
                                    });
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
                    HighlightObject(mMarkedObject, GrappleOption.CannotGrapple);
                }
            }
        }
    }

    private void HighlightObject(GameObject mMarkedObject, GrappleOption option)
    {
        Renderer renderer = mMarkedObject.GetComponent<Renderer>();
        if (renderer)
        {
            switch (option)
            {
                case GrappleOption.None:
                    renderer.material.color = Color.white;
                    break;
                case GrappleOption.CanGrapple:
                    renderer.material.color = Color.green;
                    break;
                case GrappleOption.CannotGrapple:
                    renderer.material.color = Color.red;
                    break;

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
        mIsGrappling = false;
        mHandR.Rotate(0, -_yAngle, 0, Space.Self);
        mHandL.Rotate(0, -_yAngle, 0, Space.Self);
        Debug.Log(mHandL_defaultRotation.eulerAngles);
    }

    private void RotateHandToPoint(Transform hand, Vector3 position)
    {
        Vector3 vec = position - hand.position;
        _yAngle = Vector3.Angle(vec, -transform.up);
        hand.Rotate(0, _yAngle, 0, Space.Self);
    }
}