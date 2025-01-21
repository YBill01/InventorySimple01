namespace GameName.Services.Cmd
{
	public interface ICommandProcessor
	{
		void RegisterHandler<T>(ICommandHandler<T> handler) where T : struct, ICommand;

		void Process<T>(T command) where T : struct, ICommand;

		void ExecuteAll();
		
		void Execute<T>() where T : struct, ICommand;
	}
}