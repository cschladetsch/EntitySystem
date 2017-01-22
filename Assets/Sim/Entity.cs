using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using Sim.Physical;
using Sim.Mental;

using Flow;

namespace Sim
{
	public class Entity : Sim.TransientBehaviour
	{
		/// <summary>
		/// Unique id given to each entity 
		/// </summary>
		public int Id;

		public GameObject BodyPrefab;
		public GameObject PerceptionsPrefab;
		public GameObject MindPrefab;
		public GameObject AgentPrefab;
		public GameObject ActorPrefab;

		public Body Body;
		public Perception Perception;
		public Mind Mind;
		public Agent Agent;
		public Actor Actor;

		private void Awake()
		{
			AddComponenets(MindPrefab, PerceptionsPrefab, BodyPrefab);

			Mind = GetComponentInChildren<Mind>();
			Perception = GetComponentInChildren<Perception>();
			Body = GetComponentInChildren<Body>();

			Kernel.Root.Add(this);
		}

		private void AddComponenets(params GameObject[] prefabs)
		{
			foreach (var prefab in prefabs)
			{
				if (prefab == null) continue;

				var comp = Instantiate(prefab);
				comp.transform.SetParent(transform, false);
			}
		}

		public override bool Equals(object x)
        {
            return (x as Entity).Id == Id;
        }

		public override int GetHashCode()
        {
            return Id;
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

