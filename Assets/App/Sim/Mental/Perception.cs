using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using Flow;

namespace App.Sim.Mental
{
	// the lowest rung in the virtual consciousness of an Agent. it provides
	// raw sensual detection and sampling of its environment to its Body
	public class Perception : EntityComponent
	{
		public float FieldOfView = 100;
		public float ViewDistance = 20;
		public int NumRayCasts = 10;

		public float AuralDistance = 50;

		private float _perceptionRadius;
		private SphereCollider _collider;
		private int _collideLayers;

		public override void Construct()
		{
			base.Construct();

			_collideLayers = MakeLayerMask("Body", "Boundary", "Obstruction");
		}

		int MakeLayerMask(params string[] layerNames)
		{
			var result = 0;
			foreach (var layer in layerNames)
			{
				result |= (1 << LayerMask.NameToLayer(layer));
			}

			return result;
		}

		public override void Begin()
		{
			base.Begin();

			_collider = GetComponent<SphereCollider>();
		}

		public IEnumerator PublishDetection(IGenerator self, IChannel<Physical.Detection> channel)
		{
			_channel = channel;
			var perceptionRadius = ViewDistance;
			var radius = Mathf.Tan(FieldOfView/2);
			_collider.radius = perceptionRadius;

			while (channel.Active)
			{
				// cast a number of random range within our field of view
				for (var n = 0;  n < NumRayCasts; ++n)
				{
					CastRandomRay(perceptionRadius, radius);
				}

				// listen for audio sources

				yield return 0;
			}
		}

		void CastRandomRay(float length, float radius)
		{
			var local = new Vector3(
				UnityEngine.Random.Range(-radius, radius), 
				Random.Range(-radius, radius), 
				1);

			var world = transform.TransformPoint(local);
			var pos = transform.position;
			var dir = (world - pos).normalized;
			var ray = new Ray(pos, dir);
			Debug.DrawLine(pos, pos + dir*ViewDistance, Color.blue, 0, false);

			CastRay(ray, length);
		}

		void CastRay(Ray ray, float length)
		{
			var hits = Physics.RaycastAll(ray, length, _collideLayers);
			foreach (var hit in hits)
			{
				// Debug.Log("hit " + hit.collider.gameObject.name);
				Debug.DrawLine(ray.origin, hit.point, Color.yellow, 0, false);

				_channel.Insert(new Physical.Detection(this, hit));
			}
		}

		IChannel<Physical.Detection> _channel;
	}
}

