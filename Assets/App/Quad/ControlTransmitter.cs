using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

namespace App.Quad
{
	/// <summary>
	/// Models a hand-held flight transmitter. Perhaps one day this could
	/// be a phone app
	/// </summary>
	public class Transmitter : MonoBehaviour
	{
		// left stick, the throttle stays where it was left. fully down is no throttle.
		public float Throttle;		// up/down on left stick
		public float Rudder;		// left/right on left stick

		// right stick auto-centers
		public float Elevator;		// up/down on right stick
		public float Ailerone;		// left/right on right stick

		private void Awake()
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void FixedUpdate()
		{
		}
	}
}

