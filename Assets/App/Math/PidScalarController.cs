
using UnityEngine;

namespace App.Math
{
    public class PidScalarController : MonoBehaviour
	{
		public float SetPoint;

		public float Kp = .1f;
		public float Ki = .05f;
		public float Kd = .01f;

		/// <summary>
		/// Calculate output from controller. 
		/// </summary>
		/// <param name="setpoint">the target value</param>
		/// <param name="pv">the current value, with feedback error</param>
		/// <param name="dt">time since last calculation</param>
		/// <returns></returns>
		private float Calculate(float pv, float dt)
		{
			// Calculate error
			float error = SetPoint - pv;

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

		private void FixedUpdate()
		{
			float val = (float)transform.position.z;
			var inc = Calculate(val, Time.fixedDeltaTime);

			// Debug.LogFormat("{2}: val:{0}, inc:{1}", val.ToString("F3"), inc.ToString("F3"), _count++);
			transform.position = new Vector3(0,0, (float)(val + inc));
		}

        float _max = 100;
        float _min = -100;
        float _pre_error;
        float _integral;
	}
}

