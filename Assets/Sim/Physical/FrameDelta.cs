using System;

using UnityEngine;

namespace Sim.Physical
{
    public struct FrameDelta
	{
		public readonly Vector3 Velocity;
		public readonly Quaternion Rotation;
		public readonly TimeSpan Delta;

		public FrameDelta(Frame a, Frame b)
		{
			this = a - b;
		}

		public FrameDelta(Vector3 v, Quaternion r, TimeSpan t)
		{
			Velocity = v;
			Rotation = r;
			Delta = t;
		}

		public static FrameDelta operator *(FrameDelta sfd, float a)
		{
			return new FrameDelta(
				sfd.Velocity*a, 
				Quaternion.SlerpUnclamped(Quaternion.identity, sfd.Rotation, a),
				TimeSpan.FromMilliseconds(sfd.Delta.Milliseconds*a));
		}

		public static FrameDelta operator /(FrameDelta sfd, float a)
		{
			return sfd*1.0f/a;
		}

		override public string ToString()
		{
			return String.Format("v:[{0}], r:{1}, t:{2}", Velocity.ToString("F3"), Rotation.ToString("F2"), Delta.TotalMilliseconds);
		}
	}

}
