using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using UniRx;

namespace App.Math
{
	public class PidScalarControllerBehaviour : MonoBehaviour
	{
		public float SetPoint = 0;
		public float P, I, D;

		private void Awake()
		{
			 _controller = new PidScalarController();
		}

		private void Start()
		{
		}

		private void FixedUpdate()
		{
			_controller.P = P;
			_controller.I = I;
			_controller.D = D;
			
			var offset = _controller.Calculate(SetPoint, transform.position.z, 1.0f/10.0f);//Time.fixedDeltaTime);
			if (_count < 100)
				Debug.LogFormat("[{0}]: val:{1}, inc: {2}", _count++, transform.position.z, offset.ToString("F3"));

			var p = transform.position;
			var val = p.z + (float)offset;
			transform.position = new Vector3(p.x, p.y, val);

			DebugGraph.Log("offset", offset);
			DebugGraph.Log("val", val);
		}
		int _count;

		private PidScalarController _controller;
	}
}

