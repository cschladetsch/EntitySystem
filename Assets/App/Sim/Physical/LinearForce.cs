using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using Flow;

namespace App.Sim.Physical
{
	public class LinearForce : EntityComponent
	{
		public AnimationCurve Curve;
		public Vector3 Direction = new Vector3(0,0,1);
		public float Magnitude = 10;
		public float TimeSpan = 1;

		override public void Begin()
		{
			Attach(Factory.NewCoroutine(Apply));
		}

		IEnumerator Apply(IGenerator self)
		{
			var interval = TimeSpan;
			var scale = 1.0f/TimeSpan;
			var time = 0.0f;
			while (time < TimeSpan)
			{
				var mag = Curve.Evaluate(time);
				var force = Direction*mag;
				Entity.Body.AddImpulseForce(force);
				time += UnityEngine.Time.deltaTime*scale;
			}

			yield break;
		}
	}
}

