using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTests2 : MonoBehaviour
{
	public Rigidbody _rigidBody;
	public Rigidbody _rigidBody2;
	float _speed = 1.0f;
	public void Update()
	{
		var pos = _rigidBody.position;
		pos.z += Time.fixedDeltaTime * _speed;
		_rigidBody.MovePosition(pos);
		Debug.Log("Velo1:" + _rigidBody.velocity.ToString());
		var pos2 = _rigidBody2.position;
		pos2.z = pos.z;
		_rigidBody2.position = pos2;
		Debug.Log("Velo2:" + _rigidBody2.velocity.ToString());
	}
}
