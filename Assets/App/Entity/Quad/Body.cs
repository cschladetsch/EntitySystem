using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

namespace App.Quad
{
	[RequireComponent(typeof(BoxCollider))]
	public class Body : MonoBehaviour
	{
		public FlightController Controller;

		private void Awake()
		{
			_collider = GetComponent<BoxCollider>();
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

		private BoxCollider _collider;
	}
}

