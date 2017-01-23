using UnityEngine;

namespace App.Math
{
    public class PidVector3Controller
	{
		public Vector3 SetPoint;
		public float Kp;
		public float Ki;
		public float Kd;

		public PidVector3Controller()
		{
			CreateControllers();
		}

		public PidVector3Controller(float p, float i, float d)
		{
			Kp = p;
			Ki = i;
			Kd = d;
		}

		public Vector3 Calculate(Vector3 pv, float dt)
		{
			float x = _controllers[0].Calculate(pv.x, dt);
			float y = _controllers[1].Calculate(pv.y, dt);
			float z = _controllers[2].Calculate(pv.z, dt);

			return new Vector3(x,y,z);
		}

		void CreateControllers()
		{
			_controllers = new[] {
				new PidScalarController(Kp, Ki, Kd),
				new PidScalarController(Kp, Ki, Kd),
				new PidScalarController(Kp, Ki, Kd)
			};
		}

		private PidScalarController[] _controllers;	
	}
}
