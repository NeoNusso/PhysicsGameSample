using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class AddImpulse : MonoBehaviour
{
	public Rigidbody _body;
	public Vector3 _impluse;
	public ForceMode _forceMode;
	public void AddForce()
	{
		_body.AddForce(_impluse, _forceMode);
	}
#if UNITY_EDITOR
	[CustomEditor(typeof(AddImpulse))]
	public class AddImpluseEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (GUILayout.Button("AddImp"))
			{
				var tgt = target as AddImpulse;
				tgt.AddForce();
			}
		}
	}
#endif
}
