using Cysharp.Threading.Tasks;
using GameName.Data;
using GameName.Gameplay.Features.Stock;
using GameName.Services.Cmd;
using UnityEngine;

namespace GameName.Gameplay.Cmd
{
	public class StockTransferCommandHandler : CommandHandler<StockTransferCommand>
	{
		private UniTask<bool> _cachedReturn = UniTask.FromResult(true);

		private StockTransferBehaviour _stockTransfer;
		private SharedData _sharedData;

		public StockTransferCommandHandler(StockTransferBehaviour stockTransfer, SharedData sharedData)
		{
			_stockTransfer = stockTransfer;
			_sharedData = sharedData;
		}

		public override UniTask<bool> Execute(StockTransferCommand command)
		{
			StockViewItemData sourceStockViewItem = command.source;
			StockItem item = new StockItem
			{
				id = sourceStockViewItem.item.id,
				Quantity = 1
			};

			if (item.inactive || !command.target.Stock.HasEmpty(item, out int slotIndex, out int itemIndex, out bool isEmpty, true))
			{
				sourceStockViewItem.stockView.TransferReject(sourceStockViewItem);

				return _cachedReturn;
			}

			if (sourceStockViewItem.stockView.TryTransferTake(sourceStockViewItem, out Transform sourceTransform))
			{
				if (command.target.TryTransferAdd(item, out StockViewItemData targetStockViewItem, out Transform targetTransform))
				{
					float transferDuration = sourceStockViewItem.stockView.Stock.Config.transferDuration + targetStockViewItem.stockView.Stock.Config.transferDuration;

					if (transferDuration > 0.0f)
					{
						command.target.Stock.TryInactivateItem(targetStockViewItem.slotIndex, targetStockViewItem.itemIndex, true);

						StockTransferData sourceTransferData = sourceStockViewItem.stockView.TransferData(sourceStockViewItem);
						StockTransferData targetTransferData = targetStockViewItem.stockView.TransferData(targetStockViewItem);

						ItemData itemData = _sharedData.GetItemData(item.id);

						_stockTransfer.Process(
							sourceStockViewItem,
							targetStockViewItem,
							itemData.view.transferredPrefab,
							itemData,
							sourceTransform,
							targetTransform,
							transferDuration);
					}
				}
			}

			return _cachedReturn;
		}
	}
}