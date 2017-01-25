using System;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using Flow;

namespace App.Sim
{
	public class TransientBehaviour : MonoBehaviour, ITransient
	{
		public Entity Entity { get { return _entity; } }
		public IKernel Kernel { get { return Entity.Kernel; } set { Entity.Kernel = value; } }
		public IFactory Factory { get { return Kernel.Factory; } }

        public event TransientHandler Completed;
        public event TransientHandlerReason WhyCompleted;
        public event NamedHandler NewName;

        public bool Active { get { return _active; } }
        public string Name { get { return name; } set { name = value; } }

		protected void StartNow(Entity ent)
		{
			_entity = ent;
			Debug.LogFormat("TransientBehaviour.StartNew {0} {1}", Kernel, Factory);
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

		private Entity _entity;
		private bool _active = true;
	}
}
