using System;
using System.Collections.Generic;
// using System.Linq;
using NUnit.Framework;
using Flow;
using UnityEngine;

namespace TestFlow
{
	[TestFixture()]
	[Ignore]
	public class TestSequence
	{
		[Test()]
		public void TestSequence1()
		{
			var kernel = Create.NewKernel();

			var total = 0;
			var last = 0;
			kernel.Factory.NewSequence(
				() => { Debug.Log(kernel.StepNumber); total++; last = kernel.StepNumber;   },
				() => { Debug.Log(kernel.StepNumber);  total++; last = kernel.StepNumber; },
				() => { Debug.Log(kernel.StepNumber);  total++; last = kernel.StepNumber; },
				() => { Debug.Log(kernel.StepNumber);  total++; last = kernel.StepNumber; }
			);

			Debug.Log(last);
			Debug.Log(kernel.Name);
			Debug.Log(kernel.Root.Name);
			// foreach (var n in kernel.Root.Contents.ToList()[0].Contents.Cast<IGenerator>())
			// {
			// 	Debug.Log(n.Name + " is a " + n.GetType().Name + "run: " + n.Running);
			// }

			kernel.Step();
			Debug.Log("Hello");
			Assert.AreEqual(0, total);
			kernel.Step();
			Assert.AreEqual(1, total);
			kernel.Step();
			Assert.AreEqual(2, total);
			kernel.Step();
			Assert.AreEqual(3, total);
			kernel.Step();
			Assert.AreEqual(4, total);
		}

		[Test()]
		public void TestParallel1()
		{
			var kernel = Create.NewKernel();

			var total = 0;
			kernel.Factory.NewParallel(
				() => total++,
				() => total++,
				() => total++,
				() => total++
          	);

			kernel.Step();
			kernel.Step();

			Assert.AreEqual (4, total);
		}
	}
}
