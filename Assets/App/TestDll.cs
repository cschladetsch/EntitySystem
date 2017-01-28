using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.Assertions;

namespace App.Math
{
	public class TestDll : MonoBehaviour
	{
		[DllImport ("LagrangeInterpolation_Plugin")]
		private static extern int TestFromCpp();

		private void Awake()
		{
			int n = TestFromCpp();
			Debug.Log(n);
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

