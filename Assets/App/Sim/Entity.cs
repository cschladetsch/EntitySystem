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
	public class Entity : MonoBehaviour, ITransient
	{
		public IKernel Kernel { get { return _kernel; } set { _kernel = value; } }
		public IFactory Factory { get { return _kernel.Factory; } }
        public bool Active { get { return _active; } }
        public string Name { get { return name; } set { name = value; } }

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

        public event TransientHandler Completed;
        public event TransientHandlerReason WhyCompleted;
        public event NamedHandler NewName;

		static int TraceLevel = 0;

		private void Awake()
		{
			CreateKernel();

			AddComponents();

			ForeachEntityComponent((c) => c.Construct(this));
		}

		void CreateKernel()
		{
			_kernel = Flow.Create.NewKernel();
			_kernel.Root.Add(this);
			_active = true;
		}

		private void AddComponents(params GameObject[] prefabs)
		{
			foreach (var prefab in prefabs)
			{
				if (prefab == null) continue;

				var comp = Instantiate(prefab);
				comp.transform.SetParent(transform, false);

				var part = comp.GetComponent<EntityComponent>();
				if (part != null)
				{
					if (TraceLevel > 1) Debug.LogFormat("{0}.{1}: Construct", name, part.GetType().Name);
					part.Construct(this);
				}
			}
		}

		private void Start()
		{
			AddComponents(MindPrefab, PerceptionsPrefab, BodyPrefab);

			Mind = GetComponentInChildren<Mind>();
			Perception = GetComponentInChildren<Perception>();
			Body = GetComponentInChildren<Body>();

			ForeachEntityComponent((c) => c.Begin());
		}

		void ForeachEntityComponent(System.Action<EntityComponent> fun) 
		{
			foreach (var entityComponent in GetComponentsInChildren<EntityComponent>())
			{
				fun(entityComponent);
			}
		}

		private void OnDestroy()
		{
			if (Active)
				Complete();
		}

        public void Complete()
        {
			if (Active)
			{
				if (Completed != null)
					Completed(this);
				_active = false;
			}

			if (gameObject != null)
            	Destroy(gameObject);

			// deletion chains can take another Kernel step to propagate
			Kernel.Step();
			var contents = Kernel.Root.Contents;
			Assert.AreEqual(contents.Count(), 0, "Object: " + name);
			if (contents.Count() > 0)
			{
				foreach (var c in contents)
				{
					Debug.LogWarningFormat("Dangling: name={0}, type{1}", c.Name, c.GetType().Name);
				}
			}
        }

        public void CompleteAfter(ITransient other)
        {
			other.Completed += (tr) => Complete();
        }

        public void CompleteAfter(TimeSpan span)
        {
            Factory.NewTimer(span).Elapsed += (tr) => Complete();
        }

		public override bool Equals(object x)
        {
            return (x as Entity).Id == Id;
        }

		public override int GetHashCode()
        {
            return Id;
        }

		private void FixedUpdate()
		{
			_kernel.Update(Time.fixedDeltaTime);
		}

		private bool _active = true;
        private IKernel _kernel;
	}
}

