using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using App.Math;

namespace App.Sim.Physical
{
	/// <summary>
	/// A detection of another object in the scene. 
	/// </summary>
	public class Detection
	{
		public Frame SelfFrame;		// the frame of the entity when it make the detection
		public Frame OtherFrame;		// the frame of the entity when it was detected
		public RaycastHit Hit;				// what was detected, and where, from perspective of Self

		public Detection()
		{
			// throw new Exception("No empty connections please");
		}

		public Detection(Mental.Perception self, RaycastHit hit)
		{
			Assert.IsNotNull(self);

			Hit = hit;

			var when = DateTime.Now;

			// TODO; use self.Kernel.Time.Now after making it a manually-adjusted DateTime field
			var selfTrans = self.transform;
			SelfFrame = new Frame(selfTrans.position, selfTrans.rotation, when);

			var otherTrans = hit.collider.gameObject.transform;
			OtherFrame = new Frame(otherTrans.position, otherTrans.rotation, when);
		}
	}
}
