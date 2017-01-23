using System;
using UnityEngine;
using UnityEngine.Assertions;

using Flow;

namespace Sim.Physical
{
	/// <summary>
	/// A frame of reference in time, orientation, and location.
	/// 
	///	Can be compared for (in)equality, subtrated, added, and interpolated.
	/// </ summary>
    public struct Frame
	{
		public readonly Vector3 Position;
		public readonly Quaternion Orientation;
		public readonly DateTime When;			// TODO: make this DateTime?

		/// <summary>
		/// Used to compare for effective equality for vectors and scalars 
		/// </summary>
		public const float Epsilon = 0.005f;

		public static Frame Identity = new Frame(Vector3.zero, Quaternion.identity);

		public Frame(Vector3 pos)
		{
			Position = pos;
			Orientation = Quaternion.identity;
			When = DateTime.Now;
		}

		public Frame(Vector3 pos, Quaternion rot)
			: this(pos)
		{
			Orientation = rot;
		}

		public Frame(Vector3 pos, Quaternion rot, DateTime time)
			: this(pos, rot)
		{
			When = time;
		}

		public Frame(Transform tr)
			: this(tr.position, tr.rotation)
		{
		}

		public Frame(Transform tr, DateTime time)
			: this(tr.position, tr.rotation)
		{
			When = time;
		}

		public override int GetHashCode()
		{
			return Position.GetHashCode() ^ Orientation.GetHashCode() ^ When.GetHashCode();
		}

		public override bool Equals(object other)
		{
			if (other == null) return false;
			if (!(other is Frame)) return false;
			return this == (Frame)other;
		}

		public override string ToString()
		{
			return string.Format("[pos:{0}, rot:{1}, when:{2}]", Position.ToString("F4"), Orientation.ToString("F2"), When.TimeOfDay);
		}

		/// <summary>
		/// Not mathematically sensible, but useful for tests 
		/// </summary>
		/// <returns></returns>
		public float Magnitude()
		{
			return Position.magnitude + Orientation.eulerAngles.magnitude;// + When.Now.Millisecond;
		}

		public static FrameDelta operator -(Frame x, Frame y)
		{
			return Subtract(x, y);
		}

		public static Frame operator +(Frame x, FrameDelta y)
		{
			return Add(x, y);
		}

		// public static SpatialFrame operator *(SpatialFrameDelta x, float alpha)
		// {
		// 	var d = x - Identity;
		// 	return x.Interpolate(d, alpha);
		// }

		public static bool operator ==(Frame x, Frame y)
		{
			var d = x - y;
			if (Mathf.Abs((float)d.Delta.TotalSeconds) > Epsilon) return false;
			if (d.Velocity.sqrMagnitude > Epsilon) return false;
			if (d.Rotation.eulerAngles.sqrMagnitude > Epsilon) return false;
			return true;
		}

		public static bool operator !=(Frame x, Frame y)
		{
			return !(x == y);
		}

		public static FrameDelta Subtract(Frame left, Frame right)
		{
			return new FrameDelta(
				left.Position - right.Position, 
				Quaternion.Inverse(right.Orientation)*left.Orientation, 
				left.When - right.When);
		}

		public Frame Interpolate(Frame other, float alpha)
		{
			return Frame.Interpolate(this, other - this, alpha);
		}

		public static Frame Add(Frame left, FrameDelta right)
		{
			return new Frame(
				left.Position + right.Velocity, 
				left.Orientation*right.Rotation, 
				left.When + right.Delta);
		}

		// TODO: note that alpha must be in [-1..1]
		public static Frame Interpolate(Frame left, FrameDelta right, float alpha)
		{
			return new Frame(
				left.Position + right.Velocity*alpha,
				Quaternion.SlerpUnclamped(left.Orientation, left.Orientation*right.Rotation, alpha),
				left.When.AddMilliseconds((float)(right.Delta.Milliseconds)*alpha));
		}
	}
}

