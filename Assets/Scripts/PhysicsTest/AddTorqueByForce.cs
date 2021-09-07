using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nussoft
{
	public class AddTorqueByForce : MonoBehaviour
	{

		public Rigidbody _body;
		public float _force;
		// Update is called once per frame
		float _time = 1.0f;
		void FixedUpdate()
		{
			if (_time > 0.0f)
			{
				_time -= Time.fixedDeltaTime;
				var center = _body.transform.position;
				var pos1 = center;
				var pos2 = center;
				pos1.x -= 1.0f;
				pos2.x += 1.0f;
				_body.AddForceAtPosition(new Vector3(0.0f, 0.0f, _force), pos1);
				_body.AddForceAtPosition(new Vector3(0.0f, 0.0f, -_force), pos2);
			}
		}
	}
}