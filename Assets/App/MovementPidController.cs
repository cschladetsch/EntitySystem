using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using VacuumBreather;

namespace App
{
	public class MovementPidController : MonoBehaviour
	{
		public float P = 5;
		public float I = 1;
		public float D = 0.00f;

		public float Target = 20;

		PidController _pidController;
		Rigidbody _rigidbody;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_rigidbody.centerOfMass = Vector3.zero;
			_pidController = new PidController(P, I, D);
		}

		float _lastError;

// see https://gist.github.com/bradley219/5373998
// for how to deal with delta error term
        private void FixedUpdate()
        {
            _pidController.Kp = P;
            _pidController.Ki = I;
            _pidController.Kd = D;

			var error = (Target - transform.position.z);
			var delta = error - _lastError;
			var output = _pidController.ComputeOutput(error, delta, Time.fixedDeltaTime);

			// apply output
			Debug.LogFormat("error:{0}, delta={1}, output={2}", error, delta, output);
			var current = transform.position;
			transform.position = new Vector3(current.x, current.y, current.z + output);

			_lastError = error;
		}
	}
}

