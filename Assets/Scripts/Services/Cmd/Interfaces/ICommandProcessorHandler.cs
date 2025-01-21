using Cysharp.Threading.Tasks;

namespace GameName.Services.Cmd
{
	public interface ICommandProcessorHandler
	{
		void Execute();
	}
}