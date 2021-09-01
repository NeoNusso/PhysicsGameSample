using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class AngleToRadian : MonoBehaviour
{
   
	[CustomEditor(typeof(AngleToRadian))]
	class AngleToRadianEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			var atr = (target as AngleToRadian);
			var angles = atr.transform.localEulerAngles;
			angles *= Mathf.Deg2Rad;
			EditorGUILayout.LabelField("angles:" + angles.z.ToString("F"));
		}
	}
}
