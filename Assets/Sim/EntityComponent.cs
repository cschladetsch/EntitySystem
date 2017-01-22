using UnityEngine;
using UnityEngine.Assertions;

namespace Sim
{
    public class EntityComponent : MonoBehaviour
	{
		public Entity Entity { get { return _entity; } }
		public Flow.IKernel Kernel { get { return Entity.Kernel; } }
		public Flow.IFactory Factory { get { return Kernel.Factory; } }
		public int TraceLevel = 0;

		private void Awake()
		{
			Construct();
		}

		private void Start()
		{
			_entity = GetComponent<Entity>();
			if (_entity != null)
				return;

			_entity = GetComponentInParent<Entity>();
			Assert.IsNotNull(_entity, "All entity components must have an Entity ancestor");
			Begin();
		}

		private void OnDestroy()
		{
			if (TraceLevel > 1) Debug.Log("Destroy: " + name);
		}

		protected virtual void Construct()
		{
			if (TraceLevel > 2) Debug.Log("Construct: " + name);
		}

		protected virtual void Begin()
		{
			if (TraceLevel > 2) Debug.Log("Begin: " + name);
		}

		public Tr Attach<Tr>(Tr transient) where Tr : class, Flow.ITransient
		{
			Assert.IsNotNull(transient);

			Entity.Completed += (tr) => transient.Complete();
			return transient;
		}

		private Entity _entity;
	}
}

