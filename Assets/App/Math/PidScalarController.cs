
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

			// derivative term - this is transient
			float delta = (error - _lastError);
			float derivative =  delta / dt;
			float Dout = D * derivative;

			// Calculate total output
			float output = Pout + Iout + Dout;

			// Restrict to max/min
			output = Mathf.Clamp(output, _min, _max);

			// Save error to previous error
			_lastError = error;

			return output;
		}

		int _count;

		// private void FixedUpdate()
		// {
		// 	float val = (float)transform.position.z;
		// 	var inc = Calculate(val, Time.fixedDeltaTime);

		// 	// Debug.LogFormat("{2}: val:{0}, inc:{1}", val.ToString("F3"), inc.ToString("F3"), _count++);
		// 	transform.position = new Vector3(0,0, (float)(val + inc));
		// }

        float _max = 100;
        float _min = -100;
        float _lastError;
        float _integral;
	}
}

