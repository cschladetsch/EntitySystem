using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using Flow;
using CoLib;

namespace App.Sim.Mental
{
	public class Actor : TransientBehaviour
	{
		public float Mass;					// total mass of the entity
		public Vector3 ImpulseForce;		// instantaneous force applied next physics tick (not using FixedUpdate!)

		private void FixedUpdate()
		{
			// temporary: entity will update at different rates depending on speed 
			// and proximity to other bodies
			// _kernel.Update(Time.fixedDeltaTime);
		}

    }
}

