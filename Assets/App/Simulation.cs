using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

namespace App.Sim
{
    public class Simulation : MonoBehaviour
	{
		public int Trace = 0;
		public GameObject[] Prefabs;

		private void Awake()
		{
			if (Trace > 2) Debug.Log("Simulation.Awake");
		}

		private void Start()
		{
			if (Trace > 2) Debug.Log("Simulation.Start");
		}

		private void Update()
		{
			if (Trace > 2) Debug.Log("Simulation.Update");
		}

		private void FixedUpdate()
		{
			if (Trace > 2) Debug.Log("Simulation.Fixed");
		}

		public T Create<T>(int prefabIndex) where T : Component
		{
			Assert.IsTrue(prefabIndex >= 0 && prefabIndex < Prefabs.Length);

			var go = Instantiate(Prefabs[prefabIndex]);
			go.transform.SetParent(transform, false);

			var ent = go.GetComponent<Entity>();
			Assert.IsNotNull(ent);
			if (ent != null)
			{
				ent.Id = ++_nextKey;
				_entities[ent.Id] = ent;
			}

			return go.GetComponent<T>();
		}

        internal void Remove(int id)
        {
            if (!_entities.ContainsKey(id)) return;

			_entities.Remove(id);
        }

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		void OnDestroy()
		{
			// foreach (var e in _entities)
			// {
			// 	e.Key.
			// }
		}

		private int _nextKey;
		private Dictionary<int, Entity> _entities = new Dictionary<int, Entity>();
    }
}

