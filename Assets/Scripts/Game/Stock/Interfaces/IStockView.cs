using GameName.Data;
using UnityEngine;

namespace GameName.Gameplay.Features.Stock
{
	public interface IStockView
	{
		Stock Stock { get; }

		StockTransferData TransferData(StockViewItemData stockViewItemData);

		bool TryGetItem(int slotIndex, int itemIndex, out StockViewItemData stockViewItemData);

		bool TryTransferAdd(StockItem stockItem, out StockViewItemData stockViewItemData, out Transform transform);
		bool TryTransferTake(StockViewItemData stockViewItemData, out Transform transform);

		void TransferDone(StockViewItemData stockViewItemData);

		void TransferReject(StockViewItemData stockViewItemData);
	}
}