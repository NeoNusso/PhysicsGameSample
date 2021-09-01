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
		//Quaternion _defaultCameraRot;
		Vector3 _cameraAngle;
		float _defaultCamX;
		private void Start()
		{
			var camerarot = _camera.transform.rotation;
			_rootToCamera = Quaternion.Inverse(camerarot) * (_camera.transform.position - _rootRigidbody.position);
			_cameraAngle = _camera.transform.rotation.eulerAngles;
			_defaultCamX = _cameraAngle.x;
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
				var angle = _cameraAngle.x - _defaultCamX;
				_hands[i].localPosition = Quaternion.Euler(angle, 0.0f, 0.0f) * pos;
			}
			_rootRigidbody.AddTorque(new Vector3(0.0f, _torqueY* _torqueMul, 0.0f), ForceMode.Acceleration);
			float vertical = Mathf.Clamp01(_rootRigidbody.transform.up.y);
			_rootRigidbody.AddForce(_moveDirection * _supportAccel* vertical, ForceMode.Acceleration);
			if (Input.GetKey(KeyCode.Space))
			{
				_rootRigidbody.AddForce(Vector3.up * 10.0f, ForceMode.Acceleration);
			}
		}
		float _supportAccel = 2.0f;
		float _torqueY;
		
		public float _cameraSpeed = 60.0f;
		Vector3 _moveDirection;
		private void Update()
		{
			var direction = new Vector3( Input.GetAxis("Horizontal"),  0.0f, Input.GetAxis("Vertical"));
			direction = _camera.transform.TransformVector(direction);
			direction.y = 0.0f;
			
			if (direction.sqrMagnitude > 1.0f)
			{
				direction.Normalize();
			}
			_moveDirection = direction;
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
			
			var mouse = new Vector3(-Mathf.Clamp(Input.GetAxis("Mouse Y"), -2.0f, 2.0f), Mathf.Clamp(Input.GetAxis("Mouse X"),-2.0f,2.0f), 0.0f );
			_cameraAngle += mouse * Time.deltaTime * _cameraSpeed;
			_cameraAngle.x = Mathf.Clamp(_cameraAngle.x, -60.0f, 60.0f);

		}
		
		public void LateUpdate()
		{
			var camerarot = Quaternion.Euler(_cameraAngle);
			_camera.transform.position = _rootRigidbody.position + camerarot * _rootToCamera;
			_camera.transform.rotation = camerarot;
		}
	}
}