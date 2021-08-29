using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneGame : MonoBehaviour
{
    public ConfigurableJoint _XJoint;
	public ConfigurableJoint _ZJoint;
	public ConfigurableJoint _YJoint;

	public ConfigurableJoint[] _carcherJoint;

	public Camera _camera;
	public Transform[] _cameraPositions;
	int _cameraIndex;
	void CameraUpdate()
	{
		var target = _cameraPositions[_cameraIndex];
		var camtr = _camera.transform;
		_camera.transform.SetPositionAndRotation(
			Vector3.Lerp(camtr.position, target.position, 0.1f), Quaternion.Slerp( camtr.rotation, target.rotation,0.1f));
	}

	// Update is called once per frame
	void JointMove(ConfigurableJoint joint, Vector3 deltapos)
	{
		var position = joint.targetPosition;
		position += deltapos;
		joint.targetPosition = position;
	}
	void JointMoveAxis(ConfigurableJoint joint, int axis, float delta,  float max, float min)
	{
		var position = joint.targetPosition;
		position[axis] = Mathf.Clamp(position[axis]+delta, min, max);
		joint.targetPosition = position;
	}
	void JointRot(ConfigurableJoint joint, Vector3 deltarot)
	{
		var rot = joint.targetRotation;
		rot *= Quaternion.Euler(deltarot);
		joint.targetRotation = rot;
	}
	float _jointMoveSpeed = 0.5f;
	float _jointRotSpeed = 15.0f;
	void RotCatch(float delta)
	{
		var rot = new Vector3(0.0f, 0.0f, delta);
		JointRot(_carcherJoint[0], rot);
		JointRot(_carcherJoint[1], -rot);
	}
	private void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.D))
		{
			JointMove(_XJoint, new Vector3(Time.fixedDeltaTime * _jointMoveSpeed, 0.0f, 0.0f));
		}
		if (Input.GetKey(KeyCode.A))
		{
			JointMove(_XJoint, new Vector3(-Time.fixedDeltaTime * _jointMoveSpeed, 0.0f, 0.0f));
		}
		if (Input.GetKey(KeyCode.W))
		{
			JointMove(_ZJoint, new Vector3(0.0f, 0.0f, Time.fixedDeltaTime * _jointMoveSpeed));
		}
		if (Input.GetKey(KeyCode.S))
		{
			JointMove(_ZJoint, new Vector3(0.0f, 0.0f, -Time.fixedDeltaTime * _jointMoveSpeed));
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			JointMoveAxis(_YJoint, 1, -Time.fixedDeltaTime * _jointMoveSpeed, 0.53f, 0.0f);
			//JointMove(_YJoint, new Vector3(0.0f, -Time.fixedDeltaTime * _jointModeSpeed, 0.0f));
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			JointMoveAxis(_YJoint, 1, Time.fixedDeltaTime * _jointMoveSpeed, 0.53f, 0.0f);
			//JointMove(_YJoint, new Vector3(0.0f, Time.fixedDeltaTime * _jointModeSpeed, 0.0f));
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			RotCatch(-Time.fixedDeltaTime * _jointRotSpeed);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			RotCatch(Time.fixedDeltaTime * _jointRotSpeed);
		}
		CameraUpdate();
	}
	float defaultDeltatime;
	bool _lowTimestep;
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			_cameraIndex ^= 1;
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			if(defaultDeltatime == 0.0f)
			{
				defaultDeltatime = Time.fixedDeltaTime;
			}
			_lowTimestep ^= true;
			if (_lowTimestep)
			{
				Time.fixedDeltaTime = 0.15f;
			}
			else
			{
				Time.fixedDeltaTime = defaultDeltatime;
			}
		}
	}
}
