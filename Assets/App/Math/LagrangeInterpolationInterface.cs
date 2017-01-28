using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;
using System.Runtime.InteropServices;

namespace App.Math
{   
	public class LagrangeInterpolationDll
	{
		[DllImport("LagrangeInterpolation_Plugin")]
		public static extern int TestFromCpp();
	}
}

