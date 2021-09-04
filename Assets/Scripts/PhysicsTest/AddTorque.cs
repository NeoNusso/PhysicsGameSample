using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTorque : MonoBehaviour
{
    //public Vector3 _axis;
    public Vector3 _torque;
    public Rigidbody _rigid;
	// Update is called once per frame
	private void Start()
	{
        //_rigid.addto(_torque, ForceMode.Force);

    }
    float _time = 1.0f;
	void FixedUpdate()
    {
        if(_time > 0.0f)
		{
            _time -= Time.fixedDeltaTime;
            var inertia = _rigid.inertiaTensor;
            _rigid.AddTorque(new Vector3(0.0f,  Mathf.PI * inertia.y, 0.0f), ForceMode.Force);
        }
    }
}
