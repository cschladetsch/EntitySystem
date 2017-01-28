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
			Frame s0 = RandomFrame();
			Frame s1 = RandomFrame();
			FrameDelta diff = s1 - s0;
			Frame sum = s0 + diff;

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
				Frame s0 = RandomFrame();
				Frame s1 = RandomFrame();

				// the systemic way
				Frame i0 = s0.Interpolate(s1, a);

				// using SpatialFrameDelta
				Frame i1 = s0 + (s1 - s0)*a;

				// the manual way
				DateTime t = s0.When + TimeSpan.FromMilliseconds((s1.When - s0.When).TotalMilliseconds*a);
				Frame half = new Frame(
					s0.Position + (s1.Position - s0.Position)*a,
					Quaternion.Slerp(s0.Orientation, s1.Orientation, a),
					t);

				Assert.IsTrue(i0 == half);
				Assert.IsTrue(i1 == half);
				Assert.IsTrue(i0 == i1);
			}
		}

		[Test]
		public void TestFrameDelta()
		{
			DateTime now = DateTime.Now;
			Frame a = new Frame(new Vector3(0,0,0), now);
			Frame b = new Frame(new Vector3(10,10,0), now + TimeSpan.FromSeconds(5));

			FrameDelta x = (b - a);
			FrameDelta c = x*0.5f;
			FrameDelta d = new FrameDelta(new Vector3(5,5,0), TimeSpan.FromSeconds(2.5f));

			Assert.AreEqual(c, d);
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
