using UnityEngine;
using UnityEditor;
using NUnit.Framework;

using App.Quad;
using App.Math;
using Mystery.Graphing;

namespace QuadTests
{
	[TestFixture]
	public class TestFlightController {

		[Test]
		public void TestWorldLinePrediction()
		{
			var go = new GameObject();
			var fc = go.AddComponent<FlightController>();

			var samples = new[] { new Vector3(0,0), new Vector3(10,10), new Vector3(20,5) };
			foreach (var f in samples)
				fc.AddFrame(f);

			var dt = 0.5f;
			var next = fc.PredictNextWorldState(dt);
			Assert.IsNotNull(next);

			// var res = next.Value;
			// Debug.Log(res.Position);
			var fo = samples[2] + (samples[2] - samples[1])*dt;
			Debug.Log(fo);
		}
	}
}
