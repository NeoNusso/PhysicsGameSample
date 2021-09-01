using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class RagdollSyncer : MonoBehaviour
{

	[System.Serializable]
	public class JointSet
	{
		public ConfigurableJoint _joint;
		Rigidbody _rigidbody;
		Quaternion _defaultJointRotInv;
		public Transform _target;
		public Transform _targetParent;
	
		public void Setup(float spring, float dumper)
		{
			_rigidbody = _joint.GetComponent<Rigidbody>();
			_defaultJointRotInv = Quaternion.Inverse(_joint.connectedBody.rotation) * _rigidbody.rotation;

#if true
			_joint.rotationDriveMode = RotationDriveMode.XYAndZ;
			
			var yzjointdrive = _joint.angularYZDrive;
			yzjointdrive.positionSpring = spring;
			yzjointdrive.positionDamper = dumper;
			_joint.angularYZDrive = yzjointdrive;

			var xjointdrive = _joint.angularXDrive;
			xjointdrive.positionSpring = spring;
			xjointdrive.positionDamper = dumper;
			_joint.angularXDrive = xjointdrive;
#else
			_joint.rotationDriveMode = RotationDriveMode.Slerp;
			var drive = _joint.slerpDrive;
			drive.positionSpring = spring;
			drive.positionDamper = dumper;
			_joint.slerpDrive = drive;
#endif
		}
	
		public void FixedUpdate()
		{
			_joint.targetRotation = CalcTargetRotation(_target.rotation, _targetParent.rotation);
			
		}

		Quaternion CalcTargetRotation(in Quaternion target, in Quaternion targetParent)
		{
			var targetrotation = Quaternion.Inverse(target) * targetParent;
			return targetrotation * _defaultJointRotInv;
		}

		public JointSet(ConfigurableJoint joint)
		{
			_joint = joint;
		}

		public void FindTarget(Transform targetRoot)
		{
			_target = targetRoot.FindByName(_joint.name);
			_targetParent = targetRoot.FindByName(_joint.connectedBody.name);
		}
	}

	public List<JointSet> _jointset = new List<JointSet>();
	public Transform _targetRoot;
	public Rigidbody _hips;
	public float _stabilizeTorque = 10.0f;
	public float _spring = 100.0f;
	public float _dumper = 1.0f;

	private void Start()
	{
		foreach (var joint in _jointset)
		{
			joint._joint.transform.parent = this.transform;
		}
		foreach (var joint in _jointset)
		{
			joint.Setup(_spring, _dumper);
		}
	}
	void FixedUpdate()
	{
		foreach (var joint in _jointset)
		{
			joint.FixedUpdate();
		}
		if (_hips)
		{
			var torque = Vector3.Cross(_hips.transform.up, Vector3.up);
			var ratio = 1.0f - Mathf.Clamp01(_hips.transform.up.y);
 			_hips.AddTorque(torque * _stabilizeTorque * (1.0f + 20.0f * ratio) , ForceMode.Acceleration);
		}
	}
	
#if UNITY_EDITOR
	public void FindJoints()
	{
		var joints = gameObject.GetComponentsInChildren<ConfigurableJoint>();
		_jointset.Clear();
		
		foreach (var joint in joints)
		{
			var jointset = new JointSet(joint);
			_jointset.Add(jointset);
			joint.axis = Vector3.right;
			joint.secondaryAxis = Vector3.up;
			EditorUtility.SetDirty(joint);
			jointset.FindTarget(_targetRoot);
		}
	}
	

	[CustomEditor(typeof(RagdollSyncer))]
	class PhysicsSyncerEditor:Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (GUILayout.Button("GetJoints"))
			{
				var tgt = target as RagdollSyncer;
				tgt.FindJoints();
			}
		}
	}
	
#endif

}
public static class UnityExtension
{

	public static Transform FindByName(this Transform obj, string targetname)
	{
		if (obj.name == targetname)
		{
			return obj;
		}
		foreach (Transform tr in obj)
		{
			var find = tr.FindByName(targetname);
			if (find)
			{
				return find;
			}
		}
		return null;
	}
}