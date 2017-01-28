﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.Assertions;

using Mystery.Graphing;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct V3
{
	float x, y, z;

	public V3(float a, float b, float c)
	{
		x = a;
		y = b;
		z = c;
	}
};

namespace App.Math
{
	public class TestDll : MonoBehaviour
	{
		private delegate void DebugCallback(string message);

		[DllImport("LagrangeInterpolation")]
		private static extern void RegisterDebugCallback(DebugCallback callback);

		[DllImport ("LagrangeInterpolation")]
		private static extern int HelloFromCpp();

		[DllImport ("LagrangeInterpolation")]
		public static extern int Entry2(
		[MarshalAs(UnmanagedType.LPArray)] V3[] points);

		[DllImport ("LagrangeInterpolation")]
		public static extern int Entry3(
		[In, Out] V3[] points);

		private void Awake()
		{
			int n = HelloFromCpp();
			Debug.Log(n);
		}

		private void Start()
		{
			// TODO RegisterDebugCallback(new DebugCallback(DebugMethod));
		}

		private static void DebugMethod(string message)
		{
			Debug.Log("C++: " + message);
		}

		private void Update()
		{
			var input = new[] { 
				new V3(0,0,0), 
				new V3(5,5,0), 
				new V3(10,-2,0) };

			int n = Entry2(input);
			Debug.Log(n);

			// Debug.Log(Entry3(input));
		}

		private void FixedUpdate()
		{
		}
	}
}

