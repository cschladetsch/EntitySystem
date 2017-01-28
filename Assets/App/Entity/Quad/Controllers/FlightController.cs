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
		public enum EMode
		{
			None,
			RateLimited = 1,
			AltitudeHold = 4,
			ReturnToHome = 8,
			AttitudeHold = 16,
		}

		public Blade FL;		// CW,  Z- spin gives lift
		public Blade FR;		// CCW, Z+ spin gives lift
		public Blade RL;		// CCW, Z+ spin gives lift
		public Blade RR;		// CW,  Z- spin gives lift

		private static int TraceLevel = 5;

		private void Awake()
		{
			TraceLevel = 3;
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

			if (TraceLevel > 1) Debug.DrawLine(avgPos, avgPos + avgForce*10, Color.yellow, 0);
			if (TraceLevel > 5) Debug.LogFormat("{0} {1} {2}", transform.position, avgPos, avgForce);

			_impulses.Clear();

			RunControllers();

			UpdateWorldLine();
		}

		void RunControllers()
		{
			foreach (var cont in GetComponentsInChildren<FlightControllerComponent>())
			{
				cont.Control();
			}
		}

		private void UpdateWorldLine()
		{
			// oldest world frame is always at end of list
			_worldLine.Add(new Frame(transform));
			if (_worldLine.Count > 3)
				_worldLine.RemoveAt(0);
		}

		// used only for testing
		public void AddFrame(Vector3 pos)
		{
			_worldLine.Add(new Frame(pos));
		}

		public Frame? PredictNextWorldState(float dt)
		{
			if (_worldLine.Count == 0)
				return null;

			var a = _worldLine[0];
			if (_worldLine.Count == 1)
				return a + (a - Frame.Identity)*dt;

			var b = _worldLine[1];
			if (_worldLine.Count == 2)
				return a + (b - a)*dt;

			// if we have three sample points, take into consideration
			// all three. 
			Frame c = _worldLine[2];
			float w0 = 0.5f;				// how much to consider first delta
			float w1 = (1.0f - w0);
			Frame m0 = a + (b - a)*w0;
			Frame m1 = m0 + (c - m0)*w1;
			Frame pred = m1 + (c - b)*dt;
			Debug.LogFormat("m0: {0}, m1: {1}, pred={2}", m0.Position,   m1.Position, pred.Position);
			return pred;
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

		private Blade[] _allBlades;
		private Rigidbody _rigidBody;
		private BoxCollider _collider;
		private List<AppliedForce> _impulses = new List<AppliedForce>();
		private List<Frame> _worldLine = new List<Frame>();
	}
}

