using GameName.Data;
using GameName.Gameplay.Features.Stock;
using System;

namespace GameName.UI
{
	public interface IUIStockView : IDisposable
	{
		event Action<int, int> OnDropItem;
		event Action<int, int, int, int> OnTransferItem;

		Stock Stock { get; }

		void InitView(Stock model, SharedData sharedData);

		void SlotsUpdate();

		void SlotUpdate(int slotIndex);
	}
}