using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using Random = UnityEngine.Random;

using NUnit.Framework;
using App.Math;

namespace MathTests
{
	[TestFixture]
	public class TestFilters
	{
		[Test]
		public void TestStepResponseButterworth()
		{
			var r = new [] { 0.4f, 0.8f, 1.0f, 1.2f, 1.4f, 1.6f, 1.8f};
			var filters = new List<ButterworthFilter>();
			var sampleRate = 30;
			foreach (var resonance in r)
			{
				filters.Add(new ButterworthFilter(Mathf.PI/8, sampleRate, ButterworthFilter.PassType.Lowpass, resonance));
			}

			float dt = 1.0f/(float)sampleRate;
			
			// var fileName = string.Format("res{0}", resonance).Replace('.', '-') + ".csv";
			var fileName = string.Format("filtered.csv");
			using (var file = new StreamWriter(fileName))
			{
				for (float s = 0; s < 100; ++s)
				{
					var t = 0.0f;
					for (int n = 0; n < filters.Count; ++n)
					{
						var v = filters[n].Update(1);
						file.Write("{0}", v);
						if (n < filters.Count - 1) file.Write(", ");
						t += dt;
					}
					file.WriteLine();
				}
			}
		}
	}
}

