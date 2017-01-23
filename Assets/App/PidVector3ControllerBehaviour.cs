using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using App.Math;

namespace App
{
	public class PidVector3ControllerBehaviour : MonoBehaviour
	{
		public Vector3 Target;

		// TODO: may want different co-efficients for the different axes
		public float P = 0.1f;
		public float I = 0.5f;
		public float D = 0.01f;

		private void Awake()
		{
			_controller = new PidVector3Controller(P, I, D);
			_controller.SetPoint = Target;
		}

		private void FixedUpdate()
		{
			var output = _controller.Calculate(transform.position, Time.fixedDeltaTime);
			transform.position = output;
		}

		private PidVector3Controller _controller;
	}
}

