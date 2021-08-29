using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
namespace Nussoft
{
	public class HumanMoveController : MonoBehaviour
	{
		public Animator _animator;

		public Rigidbody _rootRigidbody;
		public Vector3 _rootToCamera;
		public Camera _camera;
		public Transform[] _hands;
		public HandJoint[] _handjoints;
		public TwoBoneIKConstraint[] _constraints;
		Vector3[] _handsDefaultPosition;
		//public [] 
		float _pichAngle;
		float[] _armForwardRatio;
		bool[] _armForward;
		private void Start()
		{
			_rootToCamera = _camera.transform.position - _rootRigidbody.position;
			int length = _hands.Length;
			_handsDefaultPosition = new Vector3[length];
			_armForwardRatio = new float[length];
			_armForward = new bool[length];
			for (int i = 0; i< _hands.Length; ++i)
			{
				_handsDefaultPosition[i] =  _hands[i].localPosition;
			}
		}
		public float _torqueMul = 10.0f; 
		private void FixedUpdate()
		{
			for (int i = 0; i < _armForward.Length; ++i)
			{
				if (_armForward[i])
				{
					_armForwardRatio[i] = Mathf.Clamp01(_armForwardRatio[i] + Time.fixedDeltaTime * 2.0f);
				}
				else
				{
					_armForwardRatio[i] = Mathf.Clamp01(_armForwardRatio[i] - Time.fixedDeltaTime * 2.0f);
				}
				_constraints[i].weight = _armForwardRatio[i];

				var pos = _handsDefaultPosition[i];
				pos.z += _armForwardRatio[i];
				pos.y += _armForwardRatio[i] * 0.5f;
				_hands[i].localPosition = pos;
			}
			_rootRigidbody.AddTorque(new Vector3(0.0f, _torqueY* _torqueMul, 0.0f), ForceMode.Acceleration);
		}
		float _torqueY;
		private void Update()
		{
			var direction = new Vector3( Input.GetAxis("Horizontal"),  0.0f, Input.GetAxis("Vertical"));
			direction = _camera.transform.TransformVector(direction);
			direction.y = 0.0f;
			if (direction.sqrMagnitude > 1.0f)
			{
				direction.Normalize();
			}
			var rot = Quaternion.FromToRotation(_rootRigidbody.transform.forward, direction);
			var euler = rot.eulerAngles;
			_torqueY = euler.y;
			if(_torqueY > 180.0f)
			{
				_torqueY -= 360.0f;
			}
			if (_animator)
			{
				_animator.SetFloat("Speed", 2.0f * direction.magnitude);
				_animator.SetFloat("MotionSpeed", 1.0f);
			}
			for(int i = 0; i<_armForward.Length; ++i)
			{
				_armForward[i] = _handjoints[i].grabbing = Input.GetMouseButton(i);
				
			}
		}
		public void LateUpdate()
		{
			_camera.transform.position = _rootRigidbody.position + _rootToCamera;
		}
	}
}