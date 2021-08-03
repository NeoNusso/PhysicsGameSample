using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
	public Transform[] _handTransform;
	public Transform _spine;
	float _moveSpeed = 0.7f;
	float _rotSpeed = 10.0f;
	void MoveTarget(Transform t, Vector3 velocity)
	{
		var pos = t.localPosition;
		pos += velocity * Time.fixedDeltaTime;
		t.localPosition = pos;
	}
	void RotateTarget(Transform t, Vector3 velocity)
	{
		var angles = t.localRotation.eulerAngles;
		angles += velocity * Time.fixedDeltaTime;
		t.localRotation = Quaternion.Euler(angles);
	}
	static Vector3 zero = Vector3.zero;
	private void FixedUpdate()
	{
		Vector3[] velocity = new Vector3[2];
		if (Input.GetKey(KeyCode.W))
		{
			velocity[0].y += _moveSpeed;
			velocity[1].y += _moveSpeed;
		}
		if (Input.GetKey(KeyCode.S))
		{
			velocity[0].y -= _moveSpeed;
			velocity[1].y -= _moveSpeed;
		}
		if (Input.GetKey(KeyCode.D))
		{
			velocity[0].x += _moveSpeed;
			velocity[1].x -= _moveSpeed;
		}
		if (Input.GetKey(KeyCode.A))
		{
			velocity[0].x -= _moveSpeed;
			velocity[1].x += _moveSpeed;
		}
		if (Input.GetKey(KeyCode.E))
		{
			velocity[0].z += _moveSpeed;
			velocity[1].z += _moveSpeed;
		}
		if (Input.GetKey(KeyCode.Q))
		{
			velocity[0].z -= _moveSpeed;
			velocity[1].z -= _moveSpeed;
		}
		for (int i = 0; i<2; ++i)
		{
			if (velocity[i] != zero)
			{
				MoveTarget(_handTransform[i], velocity[i]);
			}
		}

		Vector3 angulerVelocity = zero;
		if (Input.GetKey(KeyCode.RightArrow))
		{
			angulerVelocity.y += _rotSpeed;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			angulerVelocity.y -= _rotSpeed;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			angulerVelocity.x += _rotSpeed;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			angulerVelocity.x -= _rotSpeed;
		}
		if (angulerVelocity != zero)
		{
			RotateTarget(_spine, angulerVelocity);
		}
		
	}
}
