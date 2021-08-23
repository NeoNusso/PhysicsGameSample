using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PhysicsTests : MonoBehaviour
{
	Vector3 startPosition;
	Vector3 startPosition2;
	public Rigidbody _rigid;
	public Rigidbody _rigid2;
	public GameObject _wall;
	private void Start()
	{
		startPosition = _rigid.position;
		startPosition2 = _rigid2.position;
	}
	public enum Operation 
	{
		None,
		MovePosition
	}
	public Operation _operation;
	Rigidbody _target;
	private void FixedUpdate()
	{
		if(_operation == Operation.MovePosition)
		{
			_rigid.MovePosition(startPosition);
			_rigid2.transform.position = startPosition2;
			_wall.SetActive(true);
			Debug.Log("Velocity:" + _rigid.velocity.ToString());
			_operation = Operation.None;
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(PhysicsTests))]
	class PhysicsTestsEditor : Editor 
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();


			if (GUILayout.Button("MovePosition"))
			{
				var pt = (target as PhysicsTests);
				pt._operation = Operation.MovePosition;
			}
		}
	}
#endif
}
