using System;

using UnityEngine;
// using UnityEngine.Assertions;
using Random = UnityEngine.Random;

using NUnit.Framework;

using App.Math;

namespace MathTests
{
	[TestFixture]
	public class TestFrame
	{
		[Test]
		public void TestOperators()
		{
			var s0 = RandomFrame();
			var s1 = RandomFrame();
			var diff = s1 - s0;
			var sum = s0 + diff;

			Assert.IsFalse(s0 == s1);
			Assert.IsTrue(s0 != s1);
			Assert.IsTrue(s0 + diff == s1);
			Debug.Log(sum);
		}

		/// <summary>
		/// Test interpolation between spatial frames at various parametersed lengths 
		/// </summary>
		[Test]
		public void TestInterpolation()
		{
			// the various interpolation factors to use
			var alphas = new[] { 0.1f, 0.2f, 0.5f, 0.75f, 1.0f };
			foreach (var a in alphas)
			{
				var s0 = RandomFrame();
				var s1 = RandomFrame();

				// the systemic way
				var i0 = s0.Interpolate(s1, a);

				// using SpatialFrameDelta
				var i1 = s0 + (s1 - s0)*a;

				// the manual way
				var t = s0.When + TimeSpan.FromMilliseconds((s1.When - s0.When).TotalMilliseconds*a);
				var half = new Frame(
					s0.Position + (s1.Position - s0.Position)*a,
					Quaternion.Slerp(s0.Orientation, s1.Orientation, a),
					t);

				Assert.IsTrue(i0 == half);
				Assert.IsTrue(i1 == half);
				Assert.IsTrue(i0 == i1);
			}
		}

		private Frame RandomFrame() 
		{
			return new Frame(
				Random.onUnitSphere*Random.Range(1, 10), 
				Random.rotation, 
				DateTime.Now + TimeSpan.FromMilliseconds((float)Random.Range(0,1000)));
		}
	}
}
