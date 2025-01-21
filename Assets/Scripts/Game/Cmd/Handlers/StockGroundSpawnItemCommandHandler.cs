using Cysharp.Threading.Tasks;
using GameName.Data;
using GameName.Gameplay.Features.Stock;
using GameName.Services.Cmd;
using System.Collections.Generic;
using UnityEngine;

namespace GameName.Gameplay.Cmd
{
	public class StockGroundSpawnItemCommandHandler : CommandHandler<StockGroundSpawnItemCommand>
	{
		private Queue<StockGroundSpawnItemCommand> _queueAwait = new();

		private GameWorld _gameWorld;
		private SharedData _sharedData;

		public StockGroundSpawnItemCommandHandler(GameWorld gameWorld, SharedData sharedData)
		{
			_gameWorld = gameWorld;
			_sharedData = sharedData;
		}

		public override async void Execute()
		{
			if (_queue.Count == 0)
			{
				return;
			}

			while (_queue.Count > 0)
			{
				_queueAwait.Enqueue(_queue.Dequeue());
			}

			while (_queueAwait.Count > 0 && await Execute(_queueAwait.Dequeue()));
		}

		public override async UniTask<bool> Execute(StockGroundSpawnItemCommand command)
		{
			GroundStorage groundStorage = _gameWorld.GroundStorage;

			((GroundStorageStockView)groundStorage.StockView).itemSpawnTransformHelper = command.spawnTransformHelper;
			if (!groundStorage.Stock.TryAdd(new StockItem
			{
				id = command.id,
				inactive = false,
				Quantity = 1
			}, out int slotIndex, out int itemIndex))
			{
				Debug.Log($"spawn fail. id: {command.id}");
			}

			await UniTask.WaitForSeconds(0.05f);

			return true;
		}
	}
}