using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using App.Sim.Physical;
using App.Sim.Mental;

using Flow;

namespace App.Sim
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

		private void Start()
		{
			_kernel = Create.NewKernel();
			_kernel.Root.Add(this);

			// start the inherited TransientBehaviour
			StartNow(this);

			AddComponenets(MindPrefab, PerceptionsPrefab, BodyPrefab);

			Mind = GetComponentInChildren<Mind>();
			Perception = GetComponentInChildren<Perception>();
			Body = GetComponentInChildren<Body>();

			// start all components that derive from EntityComponent
			foreach (var comp in GetComponentsInChildren<EntityComponent>())
				comp.StartNow(this);
		}

		private void AddComponenets(params GameObject[] prefabs)
		{
			foreach (var prefab in prefabs)
			{
				if (prefab == null) continue;

				var comp = Instantiate(prefab);
				comp.transform.SetParent(transform, false);

				var part = comp.GetComponent<EntityComponent>();
				if (part != null)
				{
					Debug.LogFormat("{0}.{1}: Start", name, part.GetType().Name);
					part.Construct();
				}
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

		private void Update()
		{
		}

		private void FixedUpdate()
		{
			_kernel.Update(Time.fixedDeltaTime);
		}

        private IKernel _kernel;
	}
}

