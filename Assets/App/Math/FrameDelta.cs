using System;

using UnityEngine;

namespace App.Math
{
    public struct FrameDelta
	{
		public readonly Vector3 Velocity;
		public readonly Quaternion Rotation;
		public readonly TimeSpan Delta;

		public static float Epsilon = Frame.Epsilon;

		public Vector3 Position
		{
			get { return Velocity; }
		}

		public FrameDelta(Frame a, Frame b)
		{
			this = a - b;
		}

		public FrameDelta(Vector3 v, TimeSpan span)
		{
			Velocity = v;
			Delta = span;
			Rotation = Quaternion.identity;
		}

		public FrameDelta(Frame a, Frame b, TimeSpan span)
			: this(a,b)
		{
			Delta = span;
		}

		public FrameDelta(Vector3 v, Quaternion r, TimeSpan t)
		{
			Velocity = v;
			Rotation = r;
			Delta = t;
		}

		public override bool Equals (object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;
			
			if (ReferenceEquals(this, obj))
				return true;

			if (obj is FrameDelta)
				return (FrameDelta)obj == this;

			return base.Equals (obj);
		}
		
		public override int GetHashCode()
		{
			return Position.GetHashCode() ^ Rotation.GetHashCode() ^ Delta.GetHashCode();
		}

		public static bool operator !=(FrameDelta left, FrameDelta right)
		{
			return !(left == right);
		}

		public static bool operator ==(FrameDelta left, FrameDelta right)
		{
			if (Quaternion.Angle(left.Rotation, right.Rotation) > Epsilon) return false;
			if ((right.Position - left.Position).magnitude > Epsilon) return false;
			return System.Math.Abs(right.Delta.TotalMilliseconds - left.Delta.TotalMilliseconds) < Epsilon;
		}

		public static FrameDelta operator *(FrameDelta frameDelta, float a)
		{
			var millis = ((float)frameDelta.Delta.TotalMilliseconds)*a;
			return new FrameDelta(
				frameDelta.Velocity*a, 
				Quaternion.SlerpUnclamped(Quaternion.identity, frameDelta.Rotation, a),
				TimeSpan.FromMilliseconds(millis));
		}

		public static FrameDelta operator /(FrameDelta sfd, float a)
		{
			return sfd*(1.0f/a);
		}

		override public string ToString()
		{
			return String.Format("v:[{0}], r:{1}, t:{2}", Velocity.ToString("F3"), Rotation.ToString("F2"), Delta.TotalSeconds);
		}
	}

}
