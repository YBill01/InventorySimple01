using Cysharp.Threading.Tasks;

namespace GameName.Services.Cmd
{
	public interface ICommandHandler<T> : ICommandProcessorHandler where T : struct, ICommand
	{
		void Process(T command);

		UniTask<bool> Execute(T command);
	}
}