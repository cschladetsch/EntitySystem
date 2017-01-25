using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using Flow;

namespace App.Sim
{
	public class BirdBody : EntityComponent
	{
		public GameObject LeftWing;
		public GameObject RightWing;

		public float MaxWingAngle = 60;

		private float WingFlapRate;		// full flap per second

		public override void Begin()
		{
			base.Begin();

			_rigidBody = Entity.GetComponentInChildren<Rigidbody>();
			Assert.IsNotNull(_rigidBody);

			var flapper = Entity.Factory.NewCoroutine(FlapWings);
			Attach(flapper);
		}

		IEnumerator FlapWings(IGenerator self)
		{
			yield break;
		}

		public Rigidbody _rigidBody;
	}
}

