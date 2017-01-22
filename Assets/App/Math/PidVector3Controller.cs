using UnityEngine;

namespace App.Math
{
    public class PidVector3Controller : MonoBehaviour
	{
		public Vector3 SetPoint;
		public float Kp;
		public float Ki;
		public float Kd;

		public Vector3 Calculate(Vector3 pv, float dt)
		{
			// Calculate error
			float error = (SetPoint - pv).magnitude;

			// Proportional term
			float Pout = Kp * error;

			// Integral term
			_integral += error * dt;
			float Iout = Ki * _integral;

			// Derivative term
			float derivative = (error - _pre_error) / dt;
			float Dout = Kd * derivative;

			// Calculate total output
			float output = Pout + Iout + Dout;

			// Restrict to max/min
			if( output > _max )
				output = _max;
			else if( output < _min )
				output = _min;

			// Save error to previous error
			_pre_error = error;

			return output;
		}

        float _max = 100;
        float _min = -100;
        float _pre_error;
        float _integral;
	}
}

