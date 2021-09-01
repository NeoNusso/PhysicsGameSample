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
			Vector3.Lerp(camtr.position, target.position, 0.1f), Quaternion.Slerp(camtr.rotation, target.rotation, 0.1f));
	}

	// Update is called once per frame
	void JointMove(ConfigurableJoint joint, Vector3 deltapos)
	{
		var position = joint.targetPosition;
		position += deltapos;
		position.x = Mathf.Max(0.0f, position.x);
		position.z = Mathf.Max(0.0f, position.z);
		joint.targetPosition = position;
	}
	void JointMoveAxis(ConfigurableJoint joint, int axis, float delta, float max, float min)
	{
		var position = joint.targetPosition;
		position[axis] = Mathf.Clamp(position[axis] + delta, min, max);
		joint.targetPosition = position;
	}
	float _catcherAngle;
	void JointRot(ConfigurableJoint joint, Vector3 angles)
	{
		joint.targetRotation = Quaternion.Euler(angles);
	}
	float _jointMoveSpeed = 0.5f;
	float _jointRotSpeed = 15.0f;
	void RotCatch(float delta)
	{
		_catcherAngle += delta;
		JointRot(_carcherJoint[0], new Vector3(0.0f, 0.0f, _catcherAngle));
		JointRot(_carcherJoint[1], new Vector3(0.0f, 0.0f, -_catcherAngle));
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
		StateFixedUpdate();
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
			if (defaultDeltatime == 0.0f)
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
		StateUpdate();
	}

	enum State
	{
		Idle,
		Right,
		WaitForward,
		Forward,
		OpenArm,
		Down,
		CloseArm,
		Up,
		Return,
		FinalOpenArm,
		FinalCloseArm
	}
	State _state;
	void StateUpdate()
	{
		switch (_state)
		{
			case State.Idle:
				if (Input.GetKeyDown(KeyCode.Z))
				{
					_state = State.Right;
				}
				break;
			case State.Right:
				if (!Input.GetKey(KeyCode.Z))
				{
					_state = State.WaitForward;
					_cameraIndex = 1;
				}
				break;
			case State.WaitForward:
				if (Input.GetKeyDown(KeyCode.X))
				{
					_state = State.Forward;
				}
				break;
			case State.Forward:
				if (!Input.GetKey(KeyCode.X))
				{
					_state = State.OpenArm;
					_cameraIndex = 0;
				}
				break;
		}
	}
	void StateFixedUpdate()
	{
		switch (_state) 
		{
			case State.Right:
				JointMove(_XJoint, new Vector3(Time.fixedDeltaTime * _jointMoveSpeed*0.5f, 0.0f, 0.0f));
				break;
			case State.Forward:
				JointMove(_ZJoint, new Vector3(0.0f, 0.0f, Time.fixedDeltaTime * _jointMoveSpeed * 0.5f));
				break;
			case State.OpenArm:
				RotCatch(-Time.fixedDeltaTime * _jointRotSpeed);
				if(_catcherAngle < -40.0f)
				{
					_state = State.Down;
				}
				break;
			case State.Down:
				JointMoveAxis(_YJoint, 1, Time.fixedDeltaTime * _jointMoveSpeed, 0.53f, 0.0f);
				if(_YJoint.targetPosition[1] >= 0.53f)
				{
					_state = State.CloseArm;
				}
				break;
			case State.CloseArm:
				RotCatch(Time.fixedDeltaTime * _jointRotSpeed);
				if (_catcherAngle >= 0.0f)
				{
					_state = State.Up;
				}
				break;
			case State.Up:
				JointMoveAxis(_YJoint, 1, -Time.fixedDeltaTime * _jointMoveSpeed, 0.53f, 0.0f);
				if (_YJoint.targetPosition[1] <= 0.0f)
				{
					_state = State.Return;
				}
				break;
			case State.Return:
				JointMove(_XJoint, new Vector3(-Time.fixedDeltaTime * _jointMoveSpeed * 0.5f, 0.0f, 0.0f));
				JointMove(_ZJoint, new Vector3(0.0f, 0.0f, -Time.fixedDeltaTime * _jointMoveSpeed * 0.5f));
				if(_XJoint.targetPosition.x <= 0.0f && _ZJoint.targetPosition.z <= 0.0f)
				{
					_state = State.FinalOpenArm;
				}
				break;
			case State.FinalOpenArm:
				RotCatch(-Time.fixedDeltaTime * _jointRotSpeed);
				if (_catcherAngle < -40.0f)
				{
					_state = State.FinalCloseArm;
				}
				break;
			case State.FinalCloseArm:
				RotCatch(Time.fixedDeltaTime * _jointRotSpeed);
				if (_catcherAngle >= 0.0f)
				{
					_state = State.Idle;
				}
				break;
		}

	}
}