
using UnityEngine;

namespace App
{
    public class PidController : MonoBehaviour
	{
		public double SetPoint;
		public double Kp; 
		public double Ki; 
		public double Kd; 

		public void Construct(double max, double min)
		{
			_max = max;
			_min = min;
			_pre_error = 0;
			_integral = 0;
		}

		/// <summary>
		/// Calculate output from controller. 
		/// </summary>
		/// <param name="setpoint">the target value</param>
		/// <param name="pv">the current value, with feedback error</param>
		/// <param name="dt">time since last calculation</param>
		/// <returns></returns>
		private double Calculate(double setpoint, double pv, double dt)
		{
			// Calculate error
			double error = setpoint - pv;

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
			var inc = Calculate(SetPoint, val, Time.fixedDeltaTime);

			Debug.LogFormat("{2}: val:{0}, inc:{1}", val.ToString("F3"), inc.ToString("F3"), _count++);
			transform.position = new Vector3(0,0, (float)(val + inc));

			if (_count == 100)
				enabled = false;
		}

		int _count;
        double _max = 100;
        double _min = -100;
        double _pre_error;
        double _integral;
	}
}

