
using UnityEngine;

namespace App.Math
{
	/// <summary>
	/// A PID controller for a single scalar value (float) 
	/// </summary>
    public class PidScalarController
	{
		public float SetPoint;

		// TODO: this allows only one set of co-efficients for each axis
		// may want another controller that as per-axis co-efficients
		public float P = .1f;
		public float I = .5f;
		public float D = .01f;

		public PidScalarController()
		{
		}

		public PidScalarController(float p, float i, float d)
		{
			P = p;
			I = i;
			D = d;
		}

		bool _first;
		
		/// <summary>
		/// Calculate output from controller. 
		/// </summary>
		/// <param name="setPoint">the target value</param>
		/// <param name="processValue">the current value, with feedback error</param>
		/// <param name="dt">time since last calculation</param>
		/// <returns>a delta to apply to the input, u(t)</returns>
		public float Calculate(float setPoint, float processValue, float dt)
		{
			// Calculate error
			float error = setPoint - processValue;

			// Proportional term - this is instantaneous
			float Pout = P * error;

			// Integral term - this is persistent
			_integral += error * dt;
			float Iout = I * _integral;

			// if (_first)
			// {
			// 	_lastError = error;
			// 	_first = false;
			// }

			// derivative term - this is transient
			float delta = (error - _lastError);
			float derivative =  delta / dt;
			float Dout = D * derivative;

			// Calculate total output
			float output = Pout + Iout + Dout;

			// Debug.LogFormat("P={0}, I={1}, D={2}, sum={3}", Pout, Iout, Dout, output);

			// Restrict to max/min
			output = Mathf.Clamp(output, _min, _max);

			// Save error to previous error
			_lastError = error;

			return output;
		}

		int _count;
        float _max = 100;
        float _min = -100;
        float _lastError;
        float _integral;
	}
}

