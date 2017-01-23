using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using UniRx;

namespace App
{
	public class PidScalarControllerBehaviour : MonoBehaviour
	{
		public float SetPoint = 0;
		public float P { get { return _controller.P;} set { _controller.P = value; } }
		public float I { get { return _controller.I;} set { _controller.I = value; } }
		public float D { get { return _controller.D;} set { _controller.D = value; } }

		private void Awake()
		{
			 _controller = new App.Math.PidScalarController();
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

