
using UnityEngine;

namespace App.Math
{
    public class PidScalarController : MonoBehaviour
	{
		public double SetPoint;

		public double Kp = .1;
		public double Ki = .05;
		public double Kd = .01;

		/// <summary>
		/// Calculate output from controller. 
		/// </summary>
		/// <param name="setpoint">the target value</param>
		/// <param name="pv">the current value, with feedback error</param>
		/// <param name="dt">time since last calculation</param>
		/// <returns></returns>
		private double Calculate(double pv, double dt)
		{
			// Calculate error
			double error = SetPoint - pv;

			// Proportional term
			double Pout = Kp * error;

			// Integral term
			_integral += error * dt;
			double Iout = Ki * _integral;

			// Derivative term
			double derivative = (error - _pre_error) / dt;
			double Dout = Kd * derivative;

			// Calculate total output
			double output = Pout + Iout + Dout;

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
			double val = (double)transform.position.z;
			var inc = Calculate(val, Time.fixedDeltaTime);

			// Debug.LogFormat("{2}: val:{0}, inc:{1}", val.ToString("F3"), inc.ToString("F3"), _count++);
			transform.position = new Vector3(0,0, (float)(val + inc));
		}

        double _max = 100;
        double _min = -100;
        double _pre_error;
        double _integral;
	}
}

