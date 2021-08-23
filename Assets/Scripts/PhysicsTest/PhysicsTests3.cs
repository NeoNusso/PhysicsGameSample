using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PhysicsTests3 : MonoBehaviour
{
    public Rigidbody _target;
    public Transform _nextpos;
    Vector3[] _positions = new Vector3[2];
    // Start is called before the first frame update
    void Start()
    {
        _positions[0] = _target.position;
        _positions[1] = _nextpos.position;
    }
    int _index;
	bool _switch;
	private void FixedUpdate()
	{
		if (_switch)
		{
			_switch = false;
			_index ^= 1;
            _target.MovePosition(_positions[_index]);
        }
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(PhysicsTests3))]
	class PhysicsTestsEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();


			if (GUILayout.Button("MovePosition"))
			{
				var pt = (target as PhysicsTests3);
				pt._switch = true;
			}
		}
	}
#endif
}
