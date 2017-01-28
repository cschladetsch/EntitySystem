using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

namespace App.Quad
{
	public class FlightControllerComponent : MonoBehaviour
	{
		public float P, I, D;

		private void Awake()
		{
			_fc = GetComponent<FlightController>();
			if (_fc == null)
				_fc = GetComponentInParent<FlightController>();

			Assert.IsNotNull(_fc, "FlightController Component needs a flight controller");

			Construct();
		}

		protected virtual void Construct()
		{
		}

		public virtual void Control()
		{
		}

		protected FlightController _fc;
	}
}
