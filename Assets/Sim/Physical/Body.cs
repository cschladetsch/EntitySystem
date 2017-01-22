using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using Flow;

using Sim;
using Sim.Mental;

namespace Sim.Physical
{
	public class Body : EntityComponent
	{
		public List<Mental.Connection> Connections { get { return _connections; } }

		public Frame? LocalFrame;

		protected override void Construct()
		{
			base.Construct();
		}

		protected override void Begin()
		{
			base.Begin();

			// listen for events from perceptions using publish/subscribe model
			if (Entity.Perception != null)
			{
				var channel = Attach(Factory.NewChannel<Detection>());
				Attach(Factory.NewCoroutine(Entity.Perception.PublishDetection, channel));
				Attach(Factory.NewCoroutine(ConsumeDetection, channel));
			}
		}

		// Process connections published by the perceptions component 
		IEnumerator ConsumeDetection(IGenerator self, IChannel<Detection> channel)
		{
			while (channel.Active)
			{
				IFuture<Detection> next = channel.Extract();
				yield return self.ResumeAfter(next);
				if (!next.Available)
					yield break;	

				var det = next.Value;
				var body = GetBody(det.Hit);
				if (body == null) 
					yield break;

				AddDetection(body, det);
			}
		}

		private void AddDetection(Body other, Detection det)
		{
			// if (!_detections.ContainsKey(other))
			// 	_detections.Add(other, new Connection(Self.Perception));

			// if (!_detections[other].Sample(det))
			// 	_detections.Remove(other);
		}

		Body GetBody(RaycastHit hit)
		{
			var go = hit.collider.gameObject;
			return go.GetComponent<Body>();
		}

		// Connections maintained to other Bodies. Each Connection maintains a collection of Detections.
		private List<Connection> _connections = new List<Connection>();
	}
}
