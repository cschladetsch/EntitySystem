using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using App.Math;

namespace App
{
	public class ButterworthFilterBehaviour : MonoBehaviour
	{
		public float Frequency = Mathf.PI/2.0f;
		public float[] Resonance = new [] { 0.2f, 0.7f, 1.2f };
		public float SampleRate = 30;

		private void Awake()
		{
			var sampleRate = (int)(1.0f/Time.fixedDeltaTime);
			Debug.Log(sampleRate);
			for (int n = 0; n < Resonance.Length; ++n)
			{
				_filters.Add(new ButterworthFilter(
					Frequency, (int)SampleRate,
					ButterworthFilter.PassType.Lowpass, Resonance[n]));
			}
		}

		private void Start()
		{
		}

		private float acc = 0.0f;
		private float setpoint = 1.0f;

		private void Update()
		{
			var values = new List<float>();
			var dt = 1.0f/SampleRate;
			foreach (var filter in _filters)
			{
				values.Add(filter.Update(setpoint));
			}

			acc += dt;
			if (acc > 5)
			{
				acc = 0;
				setpoint *= -1;
			}

			DebugGraph.Log("Val", values.ToArray(), Resonance.Select((f) => "res:" + f.ToString("F1")));

		}

		private List<ButterworthFilter> _filters = new List<ButterworthFilter>();
	}
}

