using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceTest : MonoBehaviour
{
    public Vector3 _force = new Vector3(0.0f, 0.0f, 1.0f);
    public Rigidbody _rigid;

    public ForceMode _forceMode;
    float _time = 1.0f;
    void FixedUpdate()
    {
        if (_time > 0.0f)
        {
            _time -= Time.fixedDeltaTime;
            _rigid.AddForce(_force, _forceMode);
        }
    }
}
