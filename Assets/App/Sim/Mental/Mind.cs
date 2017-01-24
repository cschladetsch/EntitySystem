using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using Flow;

namespace App.Sim.Mental
{
	/// <summary>
	/// Maintains connections to other Bodies.
	/// 
	/// Uses simulations to evaluation potential outcomes to a set of actions,
	/// evaluates them, then generates a plan in the form of a composite coro/command-delegate
	/// sequence.
	/// </summary>
	public class Mind : EntityComponent
	{
		protected override void Begin()
		{
			base.Begin();

			// the Mind should be used just for running simulations and
			// producing plans for the Entity

			// TODO: constantly evaluate outcomes of series of actions

			// TODO: periodically produce plans and modifications for the associated entity
		}

		private IEnumerator Think(IGenerator self)
		{
			yield break;

			// Vector3 averageNormal = Vector3.zero;
			// int count = 0;
			// foreach (var kv in _connections)
			// {
			// 	var cons = kv.Value;
			// 	averageNormal += cons.Select(a => a.Hit.normal).Aggregate((a,b) => a + b);
			// 	count += cons.Count;
			// }

			// var normal = averageNormal/count;
			// Debug.Log(normal);

			// var pos = Self.transform.position;
			// Debug.DrawLine(pos, pos + normal, Color.magenta, 20, false);
		}

		class Plan
		{

		}

		// private List<Plan> _plans = new List<Plan>();
	}
}

