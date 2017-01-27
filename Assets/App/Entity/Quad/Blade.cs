﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using App.Math;

namespace App.Quad
{
	// a blade on the quad. the motor is magical and doesn't exist: the blade just
	// provides a force given its rotational directional and worldspace frame
	public class Blade : MonoBehaviour
	{
		// input is from Flight Controller
		public PidScalarController PidController;

		// absolute scale of output from pid controller
		public float Gain = 1;

		// the magnitude of the force. we won't worry about motor
		// torque or RPM or angle of blades or blade length.
		// all this is summed up in 'ForceMag'
		public float ForceMag = 0;

		// which way the blade is spinning w.r.t the center of the quad
		// note that in a real quad, the lower side of the blade angle
		// always turns inwards towards the quad body
		public ESpinDirection Spin;

		public static int TraceLevel = 0;

		// the total force that this motor/blade combo provides
		public Vector3 Force 
		{ 
			get
			{
				var dir = -transform.forward;	
				return dir*ForceMag*LiftDir();
			}
		}

		public float LiftDir()
		{
			if (Spin == ESpinDirection.CW) return -1;
			return -1;
		}

		private void Awake()
		{
			TraceLevel = 1;
			_rigidBody = gameObject.GetComponentInParent<Rigidbody>();
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (TraceLevel > 1) Debug.DrawLine(transform.position, transform.position + Force.magnitude*5, Color.magenta, 0, false);
		}

		private void FixedUpdate()
		{
			_rigidBody.AddForceAtPosition(Force, transform.position, ForceMode.Impulse);
		}

		public void Stop()
		{
			ForceMag = 0;
		}

		private Rigidbody _rigidBody;
	}

	// spinning CW pushes up (applies force down relative to quad)
	// spinnging CCW is opposite
	public enum ESpinDirection
	{
		CW,
		CCW,
	}


}

