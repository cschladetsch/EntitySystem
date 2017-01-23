
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

using App.Math;

using TMPro;

namespace App.UI
{
    public class PIDControllerView : MonoBehaviour
	{
		public TextMeshProUGUI ObjectName;
		public PIDGainValueView Pgain;
		public PIDGainValueView Igain;
		public PIDGainValueView Dgain;

		struct PidEntry
		{
		}
		
		void Awake()
		{
			Assert.IsNotNull(ObjectName);
			Assert.IsNotNull(Pgain);
			Assert.IsNotNull(Igain);
			Assert.IsNotNull(Dgain);
		}

		public void Configure(PidScalarControllerBehaviour controller)
		{
			Assert.IsNotNull(controller);
			ObjectName.text = controller.name;

			Pgain.Configure(controller);
		}

		public void TargetPressed(Button sender)
		{
		}

		public void ResetPressed(Button sender)
		{
		}

		private PidScalarController _controller;
	}
}

