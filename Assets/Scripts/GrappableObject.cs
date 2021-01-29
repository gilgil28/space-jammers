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
        Light,
        Heavy,
        Special
    }

    class GrappableObject : MonoBehaviour
    {
        

        [SerializeField] GrappleType mGrappleType;
        
        public GrappleType GetGrappleType()
        {
            return mGrappleType;
        }
    }
}
