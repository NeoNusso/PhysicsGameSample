using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRotation : MonoBehaviour
{
    public Rigidbody _rigid;
    // Update is called once per frame
    void FixedUpdate()
    {
        _rigid.MoveRotation(  Quaternion.Euler(0.0f, 180.0f * Time.fixedDeltaTime, 0.0f ) * _rigid.rotation );
    }
}
