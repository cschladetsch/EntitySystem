using UnityEngine;
using UnityEngine.Assertions;

namespace App.Sim
{
	/// <summary>
	/// Base for components that reside inside an Entity 
	/// </summary>
    public class EntityComponent : MonoBehaviour
	{
		public Entity Entity { get { return _entity; } }
		public Flow.IKernel Kernel { get { return Entity.Kernel; } }
		public Flow.IFactory Factory { get { return Kernel.Factory; } }
		static int TraceLevel = 5;

		public virtual void Construct(Entity ent)
		{
			_entity = ent;
			if (TraceLevel > 2) Debug.Log("Construct: " + gameObject.name);
		}

		public virtual void Begin()
		{
			if (TraceLevel > 2) Debug.Log("Begin: " + gameObject.name);
		}

		public Tr Attach<Tr>(Tr transient) where Tr : class, Flow.ITransient
		{
			Assert.IsNotNull(transient);

			Entity.Completed += (tr) => transient.Complete();
			return transient;
		}

		private void OnDestroy()
		{
			if (TraceLevel > 1) Debug.Log("Destroy: " + name);
		}

		private Entity _entity;
	}
}

