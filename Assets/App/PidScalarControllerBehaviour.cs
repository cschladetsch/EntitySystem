using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

namespace App
{
	public class PidScalarControllerBehaviour : MonoBehaviour
	{
		public float SetPoint;
		public float P = 0.5f;
		public float I = 0.05f;
		public float D = 0.1f;
		private void Awake()
		{
			_controller = new App.Math.PidScalarController(P, I, D); 
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void FixedUpdate()
		{
			var offset = _controller.Calculate(SetPoint, transform.position.z, 0.1f);
			if (_count < 100)
				Debug.LogFormat("[{0}]: val:{1}, inc: {2}", _count++, transform.position.z, offset.ToString("F3"));

			var p = transform.position;
			var val = p.z + (float)offset;
			transform.position = new Vector3(p.x, p.y, val);
		}
		int _count;

		private App.Math.PidScalarController _controller;
	}
}

