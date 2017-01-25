using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using App.Math;
using App.Sim.Physical;

namespace App.Sim.Mental
{
	/// <summary>
	/// A Connection is a set of one or more Detections of another body over a period of time 
	/// </summary>
	public class Connection
	{
		// the entity maintaining the connection
		public Perception Perceiever;

		// the detections made of the perceived object
		public List<Detection> Detections = new List<Detection>();

		public Connection(Perception perceiever)
		{
			Perceiever = perceiever;
		}

		public bool Sample(Detection det)
		{
			Detections.Add(det);

			// TODO: trim old detections

			// TODO: return false if this connection should be deleted

			return true;
		}

		// predict location of connected detection into the future
		public Frame? ProjectForward(TimeSpan future)
		{
			var dets = Detections;
			switch (dets.Count)
			{
				case 0: return null;
				case 1: return dets[0].SelfFrame;
			}

			// TODO: could take count of more than last two detections to allow
			// for second-order derivatives of position and orientation
			// return dets[0].Frame.Interpolate(dets[1].Frame, (float)future.Milliseconds/(1000.0f));
			return null;
		}
	}
}

