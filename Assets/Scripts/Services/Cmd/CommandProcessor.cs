using System;
using System.Collections.Generic;

namespace GameName.Services.Cmd
{
	public abstract class CommandProcessor : ICommandProcessor
	{
		protected Dictionary<Type, ICommandProcessorHandler> _handlers = new();

		public void RegisterHandler<T>(ICommandHandler<T> handler) where T : struct, ICommand
		{
			_handlers.Add(typeof(T), handler);
		}

		public void Process<T>(T command) where T : struct, ICommand
		{
			if (_handlers.TryGetValue(typeof(T), out var handler))
			{
				((ICommandHandler<T>)handler).Process(command);
			}
		}

		public void ExecuteAll()
		{
			foreach (KeyValuePair<Type, ICommandProcessorHandler> handlerPair in _handlers)
			{
				handlerPair.Value.Execute();
			}
		}

		public void Execute<T>() where T : struct, ICommand
		{
			if (_handlers.TryGetValue(typeof(T), out var handler))
			{
				handler.Execute();
			}
		}
	}
}