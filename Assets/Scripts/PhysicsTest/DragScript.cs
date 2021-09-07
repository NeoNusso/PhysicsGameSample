using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour
{
    public float drag;
    public Rigidbody rig;
    // Update is called once per frame
    void FixedUpdate()
    {
        rig.velocity = rig.velocity * (1 - Time.fixedDeltaTime * drag);
    }
}
