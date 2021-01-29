using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{

    public enum GrappleType
    {
        None,
        VeryLight, //This object is merely an obstacle, use the grapple hook to push it away
        Light, //This object is a dyanimic object you can stand on, therfore you grab it and stand on it
        Heavy, //This object is a solid object that will not move, you grapple yourself onto it
        Special //This object has a special functionality when grappled
    }

    class GrappableObject : MonoBehaviour
    {
        

        [SerializeField] GrappleType mGrappleType;
        [SerializeField] Transform mPredefinedLandingPoint;
        
        public GrappleType GetGrappleType()
        {
            return mGrappleType;
        }

        public Vector3 GetPredefinedLandingPoint()
        {
            if(!mPredefinedLandingPoint)
                return Vector3.zero;

            return mPredefinedLandingPoint.position;
        }
    }
}
