using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using Flow;
using CoLib;

namespace App
{
	public class CommmandQueueGenerator : Generator
	{
		public CommmandQueueGenerator()
		{
			AddStep();
		}

		public CommmandQueueGenerator(params CommandDelegate[] cmds)
		{
			AddStep();
			Enqueue(cmds);
		}

		private void AddStep()
		{
			Stepped += (self) => StepQueue();
		}

		public void Enqueue(params CommandDelegate[] cmds)
		{
			_queue.Enqueue(cmds);
		}

		private void StepQueue()
		{
			if (_queue.Update(Kernel.Time.Delta.Seconds))
			{
				Complete();
			}
		}

		private CommandQueue _queue = new CommandQueue();
	}
}

