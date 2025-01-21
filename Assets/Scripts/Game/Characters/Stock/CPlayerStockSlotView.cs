using GameName.Data;
using GameName.Gameplay.Features.Stock;
using GameName.Gameplay.Objects;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace GameName.Gameplay
{
	public abstract class CPlayerStockSlotView : MonoBehaviour, IPausable
	{
		[SerializeField]
		protected StockTransferData m_transferData;
		public StockTransferData TransferData => m_transferData;

		protected StockSlotConfigData _config;
		public StockSlotConfigData Config => _config;

		protected CPlayerStockView _parent;
		protected StockedItemFactory _itemFactory;
		
		protected StockSlot<StockItem> _modelSlot;
		protected int _slotIndex;

		protected StockedItem[] _items;

		public StockedItem this[int index] => _items[index];

		protected QuantityComparer _quantityComparer = new QuantityComparer();

		protected SharedData _sharedData;

		[Inject]
		public void Construct(
			SharedData sharedData)
		{
			_sharedData = sharedData;
		}

		public void Init(StockSlotConfigData config, CPlayerStockView parent, StockedItemFactory itemFactory, StockSlot<StockItem> modelSlot, int slotIndex)
		{
			_config = config;
			_parent = parent;
			_itemFactory = itemFactory;
			_modelSlot = modelSlot;
			_slotIndex = slotIndex;

			_items = new StockedItem[_config.capacity];

			modelSlot.OnChanged += OnChanged;
		}

		protected virtual void OnChanged(StockSlot<StockItem> modelSlot)
		{
			
		}

		public virtual Transform GetTransform(int itemIndex)
		{
			return transform;
		}

		public virtual StockedItem Add(StockItem item, int slotIndex, int itemIndex)
		{
			return null;
		}
		public virtual void Remove(StockItem item, int itemIndex)
		{

		}

		public virtual void Clear()
		{

		}

		public void SetPause(bool pause)
		{
			for (int i = 0; i < _items.Length; i++)
			{
				_items[i]?.SetPause(pause);
			}
		}

		protected class QuantityComparer : IComparer<StockItem>
		{
			public int Compare(StockItem x, StockItem y)
			{
				return x.Quantity > y.Quantity ? -1 : x.Quantity < y.Quantity ? 1 : 0;
			}
		}
	}
}