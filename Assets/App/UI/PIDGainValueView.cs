using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

using App.Math;
using UniRx;

namespace App.UI
{
	// view of a gain for a PID co-efficient
	public class PIDGainValueView : MonoBehaviour
	{
		public string Name;
		public Text Value;
		public Slider Slider;

		private PidScalarControllerBehaviour _controller;

		public void Configure(PidScalarControllerBehaviour cont)
		{
			_controller = cont;

			Slider.OnValueChangedAsObservable()
				.Subscribe(val => {
					_controller.P = val;
					Value.text = val.ToString("F3");
				})
			;
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

