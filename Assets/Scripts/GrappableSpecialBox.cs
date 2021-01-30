using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class GrappableSpecialBox : GrappableObjectWithActivation
    {


        public override void OnGrapped()
        {
            base.OnGrapped();


        }

        private void OnCollisionEnter(UnityEngine.Collision collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * 20); 
            }
        }
    }
}
