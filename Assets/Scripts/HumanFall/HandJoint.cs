using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nussoft
{
	public class HandJoint : MonoBehaviour
	{
		ConfigurableJoint _joint;
		public bool grabbing { get; set; }
		// Start is called before the first frame update
		void OnCollisionEnter( Collision colli )
		{
			if(grabbing && _joint == null && colli.rigidbody && !colli.rigidbody.isKinematic && colli.gameObject.CompareTag("Carryable"))
			{
				var contactpoint = colli.contacts[0].point;

				var joint = gameObject.AddComponent<ConfigurableJoint>();
				joint.anchor = transform.InverseTransformPoint(contactpoint);
				joint.connectedBody = colli.rigidbody;
				joint.connectedAnchor= colli.rigidbody.transform.InverseTransformPoint(contactpoint);
				joint.xMotion = joint.yMotion = joint.zMotion = ConfigurableJointMotion.Locked;
				_joint = joint;
			}
		}
		private void FixedUpdate()
		{
			if(!grabbing && _joint)
			{
				Destroy(_joint);
				_joint = null;
			}
		}
	}
}