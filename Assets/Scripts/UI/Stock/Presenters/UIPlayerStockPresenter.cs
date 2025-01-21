using GameName.Gameplay.Cmd;
using GameName.Gameplay.Features.Stock;
using System;
using GameName.Data;
using UnityEngine;
using System.Collections.Generic;

namespace GameName.UI
{
	public class UIPlayerStockPresenter : IDisposable
	{
		private Stock _model;

		private IUIStockView _playerUIView;

		private IStockView _playerView;
		private IStockView _groundView;

		private GameCmd _gameCmd;

		private SharedData _sharedData;

		private QuantityComparer _quantityComparer = new QuantityComparer();

		public UIPlayerStockPresenter(
			Stock model,
			IUIStockView playerUIView,
			IStockView playerView,
			IStockView groundView,
			GameCmd gameCmd,
			SharedData sharedData)
		{
			_model = model;

			_playerUIView = playerUIView;
			
			_playerView = playerView;
			_groundView = groundView;

			_gameCmd = gameCmd;

			_sharedData = sharedData;

			_playerUIView.InitView(model, _sharedData);

			_playerUIView.OnDropItem += OnDropItem;
			_playerUIView.OnTransferItem += OnTransferItem;

			foreach (StockSlot<StockItem> slot in _model)
			{
				slot.OnChanged += SlotOnChanged;
			}
			_model.OnChanged += OnChanged;
			_model.OnAdd += OnAdd;
			_model.OnTake += OnTake;
			_model.OnInactivateItem += OnInactivateItem;
			_model.OnQuantityChanged += OnQuantityChanged;
		}

		private void SlotOnChanged(StockSlot<StockItem> slot)
		{
			_playerUIView.SlotsUpdate();
		}

		private void OnTransferItem(int sourceSlotIndex, int sourceItemIndex, int targetSlotIndex, int targetItemIndex)
		{
			if (_model.TryTake(sourceSlotIndex, sourceItemIndex, out StockItem sourceItem))
			{
				if (_model.TryTake(targetSlotIndex, targetItemIndex, out StockItem targetItem))
				{
					_model.Add(targetItem, sourceSlotIndex, sourceItemIndex);
				}

				_model.Add(sourceItem, targetSlotIndex, targetItemIndex);

				_model[sourceSlotIndex].Sort(_quantityComparer);
				_model[targetItemIndex].Sort(_quantityComparer);
			}
		}

		private void OnChanged(StockBase<StockItem> stock)
		{
			Debug.Log("OnChanged all");
		}
		private void OnAdd(StockItem item, int slotIndex, int itemIndex)
		{
			_playerUIView.SlotUpdate(slotIndex);
		}
		private void OnTake(StockItem item, int slotIndex, int itemIndex)
		{
			_playerUIView.SlotUpdate(slotIndex);
		}
		private void OnInactivateItem(int slotIndex, int itemIndex, bool value)
		{
			_playerUIView.SlotUpdate(slotIndex);
		}
		private void OnQuantityChanged(StockItem item, int slotIndex, int itemIndex)
		{
			_playerUIView.SlotUpdate(slotIndex);
		}
		
		private void OnDropItem(int slotIndex, int itemIndex)
		{
			if (_playerView.TryGetItem(slotIndex, itemIndex, out StockViewItemData stockViewItemData))
			{
				for (int i = 0; i < stockViewItemData.item.Quantity; i++)
				{
					_gameCmd.Process(new StockTransferCommand
					{
						source = stockViewItemData,
						target = _groundView
					});
				}
			}
		}

		public void Dispose()
		{
			_playerUIView.OnDropItem -= OnDropItem;
			_playerUIView.OnTransferItem -= OnTransferItem;
			_playerUIView.Dispose();

			foreach (StockSlot<StockItem> slot in _model)
			{
				slot.OnChanged -= SlotOnChanged;
			}
			_model.OnChanged -= OnChanged;
			_model.OnAdd -= OnAdd;
			_model.OnTake -= OnTake;
			_model.OnInactivateItem -= OnInactivateItem;
			_model.OnQuantityChanged -= OnQuantityChanged;
		}

		private class QuantityComparer : IComparer<StockItem>
		{
			public int Compare(StockItem x, StockItem y)
			{
				return x.Quantity > y.Quantity ? -1 : x.Quantity < y.Quantity ? 1 : 0;
			}
		}
	}
}