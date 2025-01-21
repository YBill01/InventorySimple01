using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace GameName.Services.Cmd
{
	public abstract class CommandHandler<T> : ICommandHandler<T> where T : struct, ICommand
	{
		protected Queue<T> _queue = new();

		public void Process(T command)
		{
			_queue.Enqueue(command);
		}

		public virtual void Execute()
		{
			while (_queue.Count > 0)
			{
				Execute(_queue.Dequeue());
			}
		}

		public abstract UniTask<bool> Execute(T command);
	}
}