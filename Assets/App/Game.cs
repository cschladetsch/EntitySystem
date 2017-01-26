using UnityEngine;
using UnityEngine.Assertions;

namespace App
{
    public class Game : MonoBehaviour
	{
		public static Game Instance;
		public static Sim.Simulation Simulation;

		private void Awake()
		{
			Instance = this;
			Simulation = FindObjectOfType<Sim.Simulation>();

			Assert.IsNotNull(Simulation, "Game must be running at least one simulation");
		}

		private void Start()
		{
			var ent0 = Simulation.Create<Sim.Entity>(0);
			Debug.Log(ent0.name);
			ent0.transform.position = new Vector3(0,0,0);

			// var ent1 = Simulation.Create<Sim.Entity>(0);
			// ent1.transform.position = new Vector3(0,10,0);
		}

		private void Update()
		{
		}

		private void FixedUpdate()
		{
		}
	}
}

