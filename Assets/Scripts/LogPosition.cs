using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nussoft
{
    public class LogPosition : MonoBehaviour
    {


        // Update is called once per frame
        void Update()
        {
			LogPosY(name + "Update");
		}
		int fixedcount;
		private void FixedUpdate()
		{
			LogPosY(name + ":FixedUpdate");
			fixedcount++;
		}
        void LogPosY(string nani)
		{
			Debug.Log(nani + transform.position.y.ToString("F2"));
		}
	}
}