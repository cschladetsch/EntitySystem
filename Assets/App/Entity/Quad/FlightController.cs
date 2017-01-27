using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using App.Math;

namespace App.Quad
{
	[RequireComponent(typeof(BoxCollider))]
	[RequireComponent(typeof(Rigidbody))]
	public class FlightController : MonoBehaviour
	{
		public Blade FL;
		public Blade FR;
		public Blade RL;
		public Blade RR;

		private Blade[] _allBlades;

		private static int TraceLevel = 5;

		private void Awake()
		{
			_allBlades = new [] { FL, FR, RL, RR };
			_rigidBody = GetComponent<Rigidbody>();
			_collider = GetComponent<BoxCollider>();
		}

		private void Start()
		{
			foreach (var blade in _allBlades)
			{
				blade.ForceMag = 2;
			}
		}

		private void Update()
		{
		}

		private void FixedUpdate()
		{
			var num = _impulses.Count;
			if (num == 0) return;

			var forceSum = Vector3.zero;
			var posSum = Vector3.zero;
			foreach (var af in _impulses)
			{
				_rigidBody.AddForceAtPosition(af.Force, af.Where, ForceMode.Impulse);	// Impulse because it is added every frame
				forceSum += af.Force;
				posSum += af.Where;
			}

			var avgPos = (posSum/num);
			var avgForce = (forceSum/num)*10;
			if (TraceLevel > 1) Debug.DrawLine(avgPos, avgPos + avgForce, Color.yellow, 0);

			Debug.LogFormat("{0} {1} {2}", transform.position, avgPos, avgForce);

			_impulses.Clear();
		}

		public void AddForceAtPosition(Vector3 force, Vector3 position)
		{
			_impulses.Add(new AppliedForce(force, position));
		}

		public void Stop()
		{
			foreach (var blade in _allBlades)
			{
				blade.Stop();
			}
		}

		struct AppliedForce
		{
			public Vector3 Force;
			public Vector3 Where;

			public AppliedForce(Vector3 f, Vector3 w)
			{
				Force = f;
				Where = w;
			}
		}

		private Rigidbody _rigidBody;
		private BoxCollider _collider;
		private List<AppliedForce> _impulses = new List<AppliedForce>();
	}
}

