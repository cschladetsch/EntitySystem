using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

using App.Math;

using TMPro;

namespace App
{
	public class PIDControllerView : MonoBehaviour
	{
		public TextMeshProUGUI ObjectName;
		public PIDGainValueView Pgain;
		public PIDGainValueView Igain;
		public PIDGainValueView Dgain;

		struct PidEntry
		{
		}
		
		void Start()
		{
		}

		public void Configure(PidVector3ControllerBehaviour controller)
		{
			ObjectName.text = controller.name;
		}

		public void TargetPressed(Button sender)
		{
		}

		public void ResetPressed(Button sender)
		{
		}

		private PidScalarController _controller;
	}
}

