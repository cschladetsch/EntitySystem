using UnityEngine;

namespace App.Math
{
    public class PidVector3Controller
	{
		// target location
		public Vector3 SetPoint;

		// pid co-efficients
		public float Kp = 0.5f;
		public float Ki = 0.05f;
		public float Kd = 0.1f;

		public PidVector3Controller()
		{
			CreateControllers();
		}

		public PidVector3Controller(float p, float i, float d)
		{
			Kp = p;
			Ki = i;
			Kd = d;

			CreateControllers();
		}

		public Vector3 Calculate(Vector3 pv, float dt)
		{
			return Calculate(SetPoint, pv, dt);
		}

		/// <summary>
		/// PID controller for a Vector3.
		/// The result is a delta to be added to the process value.
		/// </summary>
		/// <param name="setPoint"></param>
		/// <param name="pv"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
		public Vector3 Calculate(Vector3 setPoint, Vector3 pv, float dt)
		{
			float x = _controllers[0].Calculate(setPoint.x, pv.x, dt);
			float y = _controllers[1].Calculate(setPoint.y, pv.y, dt);
			float z = _controllers[2].Calculate(setPoint.z, pv.z, dt);

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
