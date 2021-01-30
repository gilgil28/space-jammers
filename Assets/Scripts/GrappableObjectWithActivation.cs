using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{



    class GrappableObjectWithActivation : GrappableObject
    {

        [SerializeField] GameObject mObjectToActivate;


        public override void OnGrapped()
        {
            if(!mObjectToActivate)
            {
                throw new Exception("Object must have an activation object");
            }
            mObjectToActivate.GetComponent<GrappableObject>().SetType(GrappleType.Heavy);
        }
    }
}
