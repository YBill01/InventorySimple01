using GameName.Core;
using GameName.Data;
using GameName.Gameplay.Cmd;
using GameName.Gameplay.Features.Stock;
using System;
using VContainer;
using VContainer.Unity;

namespace GameName.Gameplay
{
	public class GameFlow : IStartable, IPostStartable, IDisposable
	{
		//private Profile _profile;
		private GameConfigData _gameConfig;
		private SharedData _sharedData;

		private CoreFlow _coreFlow;
		private GameCmd _gameCmd;
		private Game _game;
		private GameWorld _gameWorld;
		private StockTransferBehaviour _stockTransfer;

		private IObjectResolver _resolver;

		public GameFlow(
			IObjectResolver resolver,
			GameConfigData gameConfig,
			SharedData sharedData,
			CoreFlow coreFlow,
			//VCamera vCamera,
			//CPlayer player,
			GameCmd gameCmd,
			Game game,
			GameWorld gameWorld,
			StockTransferBehaviour stockTransfer)
		{
			_resolver = resolver;

			_gameConfig = gameConfig;
			_sharedData = sharedData;

			_coreFlow = coreFlow;

			_gameCmd = gameCmd;
			//_player = player;
			_game = game;
			_gameWorld = gameWorld;
			_stockTransfer = stockTransfer;
		}

		public void Start()
		{
			_gameCmd.RegisterHandler(new StockTransferCommandHandler(_stockTransfer, _sharedData));
			_gameCmd.RegisterHandler(new StockGroundSpawnItemCommandHandler(_gameWorld, _sharedData));

		}

		public async void PostStart()
		{
			await _game.StartGame(_gameConfig.levels[0]);

			// spawn items...
			CPlayer player = _gameWorld.Player;

			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 11,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 11,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 11,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 11,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 22,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 22,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 33,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 33,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 33,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 33,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 33,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 33,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 33,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 33,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 33,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 33,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 33,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 44,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 44,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 55,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 55,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 66,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 66,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 77,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 77,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 88,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 88,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 99,
				spawnTransformHelper = player.transform
			});
			_gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = 99,
				spawnTransformHelper = player.transform
			});

			//_gameCmd.Execute<StockGroundSpawnItemCommand>();
		}

		public void Dispose()
		{
			
		}
	}
}