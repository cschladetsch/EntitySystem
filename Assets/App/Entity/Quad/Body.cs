using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

namespace App.Quad
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(BoxCollider))]
	public class Body : MonoBehaviour
	{
		public FlightController Controller;

		public static int TraceLevel;

		private void Awake()
		{
			_collider = GetComponent<BoxCollider>();
			_rigidBody = GetComponent<Rigidbody>();

			TraceLevel = 1;
		}

		private void Start()
		{
			if (TraceLevel > 0) Debug.Log("Quad COM: " + _rigidBody.centerOfMass);
		}

		private void Update()
		{
		}

		private void FixedUpdate()
		{
		}

		public void AddImpulseForce(Vector3 force)
		{

		}

		private BoxCollider _collider;
		private Rigidbody _rigidBody;
	}
}

