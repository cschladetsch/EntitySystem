using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using App.Math;

namespace App.Quad
{
	public class FlightController : MonoBehaviour
	{
		public Blade FL;
		public Blade FR;
		public Blade RL;
		public Blade RR;

		private Blade[] _allBlades;

		private void Awake()
		{
			_allBlades = new [] { FL, FR, RL, RR };
		}

		private void Start()
		{
			foreach (var blade in _allBlades)
			{
				blade.ForceMag = 2;
			}
		}

		private void Update()
		{
		}

		private void FixedUpdate()
		{
		}

		public void Stop()
		{
			foreach (var blade in _allBlades)
			{
				blade.Stop();
			}
		}
	}



}

