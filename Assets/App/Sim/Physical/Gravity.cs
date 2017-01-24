using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

namespace App.Sim.Physical
{
	public class Gravition : GlobalBehaviour
	{
		 public Vector3 Force = new Vector3(0,0,-10);

		 /// <summary>
		 /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
		 /// </summary>
		 void FixedUpdate()
		 {
			//  Self.Body.Pos
		 }
	}
}

