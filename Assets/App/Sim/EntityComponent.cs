using UnityEngine;
using UnityEngine.Assertions;

namespace App.Sim
{
    public class EntityComponent : MonoBehaviour
	{
		public Entity Entity { get { return _entity; } }
		public Flow.IKernel Kernel { get { return Entity.Kernel; } }
		public Flow.IFactory Factory { get { return Kernel.Factory; } }
		public int TraceLevel = 0;

		public void StartNow(Entity ent)
		{
		}

		private void Awake()
		{
			Construct();
			_entity = GetComponentInParent<Entity>();
			Assert.IsNotNull(_entity);

			// Construct();
		}

		private void Start()
		{
			Begin();
		}

		private void OnDestroy()
		{
			if (TraceLevel > 1) Debug.Log("Destroy: " + name);
		}

		public virtual void Construct()
		{
			if (TraceLevel > 2) Debug.Log("Construct: " + name);
		}

		public virtual void Begin()
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

