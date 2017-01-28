using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using App.Math;

namespace App.Quad
{
	public class AttitudeHoldController : FlightControllerComponent
	{
		protected override void Construct()
		{
			_yawPitchRoll = new PidVector3Controller(P, I, D);
		}

		public override void Control()
		{
			// var current = _fc.tras
		}

		private PidVector3Controller _yawPitchRoll;
	}
}

