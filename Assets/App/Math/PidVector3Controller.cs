using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

namespace App
{
	public class PidVector3Controller : MonoBehaviour
	{
		public Vector3 Target;
		public float P;
		public float I;
		public float D;

		public float Calculate(Vector3 error, float dt)
		{
			return 0;
		}
	}
}

