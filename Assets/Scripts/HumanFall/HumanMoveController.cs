using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nussoft
{
	public class HumanMoveController : MonoBehaviour
	{
		public Animator _animator;

		public Rigidbody _rootRigidbody;
		public Vector3 _rootToCamera;
		public Camera _camera;
		public Transform[] _hands;
		public Vector3[] _handsDefaultPosition;

		private void Start()
		{
			_rootToCamera = _camera.transform.position - _rootRigidbody.position;
			_handsDefaultPosition = new Vector3[_hands.Length];
			for (int i = 0; i< _hands.Length; ++i)
			{
				_handsDefaultPosition[i] =  _hands[i].localPosition;
			}
		}
		public float _torqueMul = 10.0f; 
		private void FixedUpdate()
		{
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
		}
		public void LateUpdate()
		{
			_camera.transform.position = _rootRigidbody.position + _rootToCamera;
		}
	}
}