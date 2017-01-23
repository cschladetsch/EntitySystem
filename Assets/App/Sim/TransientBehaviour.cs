using System;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using Flow;

namespace Sim
{
	public class TransientBehaviour : MonoBehaviour, ITransient
	{
		public IKernel Kernel { get { return _kernel; } set { _kernel = value; } }
		public IFactory Factory { get { return _kernel.Factory; } }
        public event TransientHandler Completed;
        public event TransientHandlerReason WhyCompleted;
        public event NamedHandler NewName;

        public bool Active { get { return _active; } }
        public string Name { get { return name; } set { name = value; } }

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

        private IKernel _kernel = Create.NewKernel();
		private bool _active = true;
	}
}
