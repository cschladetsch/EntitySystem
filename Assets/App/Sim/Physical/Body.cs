using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using Flow;

using App.Sim;
using App.Math;
using App.Sim.Mental;

namespace App.Sim.Physical
{
	public class Body : EntityComponent
	{
		public Vector3 Gravity = new Vector3(0,0,0);
		public float Mass = 1;
		public List<Mental.Connection> Connections { get { return _connections; } }

		public Frame? LocalFrame;

		public override void Construct(Entity ent)
		{
			Debug.Log(gameObject.name + ".Construct: Entity=" + Entity);
			base.Construct(ent);
		}

		public override void Begin()
		{
			base.Begin();

			// listen for events from perceptions using publish/subscribe model
			Debug.Log("Entity=" + Entity);
			Debug.Log("" + name + " , " + Entity.Perception);
			if (Entity.Perception != null)
			{
				var channel = Attach(Factory.NewChannel<Detection>());
				Attach(Factory.NewCoroutine(Entity.Perception.PublishDetection, channel));
				Attach(Factory.NewCoroutine(ConsumeDetection, channel));
			}
		}

        internal void AddImpulseForce(Vector3 force)
        {
            _totalImpulseForce += force;
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

		private void NotFixedUpdate()
		{
			var dt = UnityEngine.Time.fixedDeltaTime;

			var force = ApplyImpulseForce() + ApplyConstantForce();
			_acceration = force/Mass;
			_velocity += _acceration*dt;
			_position += _velocity*dt;

			transform.position = _position;
		}

		private Vector3 ApplyImpulseForce()
		{
			var f = _totalImpulseForce;
			_totalImpulseForce = Vector3.zero;
			return f;
		}
		
		Vector3 ApplyConstantForce()
		{
			return Gravity;
		}
		// Connections maintained to other Bodies. Each Connection maintains a collection of Detections.
		private List<Connection> _connections = new List<Connection>();

		private Vector3 _position;
		private Vector3 _velocity;
		private Vector3 _acceration;
		private Vector3 _totalImpulseForce;
		private Vector3 _totalConstantForce;
	}
}
