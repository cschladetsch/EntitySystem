using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using App.UI;
using App.Math;

namespace App
{
	public class PidScalarTestGame : MonoBehaviour
	{
		public PIDControllerView ControllerPrefab;

		private PIDControllerView _pidView;

		public GameObject UiRoot;
		public PidScalarControllerBehaviour Controlled;

		private void Awake()
		{
			Assert.IsNotNull(ControllerPrefab);
			Assert.IsNotNull(Controlled);
		}

		private void Start()
		{
			// _pidView = Instantiate<PIDControllerView>(ControllerPrefab);
			// Assert.IsNotNull(_pidView);
			// _pidView.Configure(Controlled);
			// _pidView.transform.SetParent(UiRoot.transform, false);
		}

		private void Update()
		{
		}

		private void FixedUpdate()
		{
			
		}
	}
}

