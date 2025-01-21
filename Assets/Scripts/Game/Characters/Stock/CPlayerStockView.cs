using GameName.Data;
using GameName.Gameplay.Features.Stock;
using UnityEngine;
using VContainer;

namespace GameName.Gameplay
{
	public class CPlayerStockView : MonoBehaviour, IStockView, IPausable
	{
		private StockConfigData _config;
		public StockConfigData Config => _config;

		public Stock Stock => _model;

		[SerializeField]
		private CPlayerStockSlotView[] m_slots;

		[Space]
		[SerializeField]
		private Transform m_itemsContainer;

		private Stock _model;

		private StockedItemFactory _itemFactory;

		private SharedData _sharedData;

		[Inject]
		public void Construct(
			SharedData sharedData)
		{
			_sharedData = sharedData;
		}

		private void Awake()
		{
			_itemFactory = new StockedItemFactory(m_itemsContainer);
		}

		public void InitView(Stock model)
		{
			_model = model;
			_config = _model.Config;

			for (int i = 0; i < _config.slots.Length; i++)
			{
				m_slots[i].Init(_config.slots[i], this, _itemFactory, _model[i], i);
			}

			_model.OnAdd += OnAdd;
			_model.OnTake += OnTake;
			_model.OnInactivateItem += OnInactivateItem;

			Filling();
		}

		public void Filling()
		{
			//TODO fill items...
		}

		private void OnAdd(StockItem item, int slotIndex, int itemIndex)
		{
			if (m_slots[slotIndex][itemIndex] != null)
			{
				return;
			}

			m_slots[slotIndex].Add(item, slotIndex, itemIndex);
		}

		private void OnTake(StockItem item, int slotIndex, int itemIndex)
		{
			m_slots[slotIndex].Remove(item, itemIndex);
		}

		private void OnInactivateItem(int slotIndex, int itemIndex, bool value)
		{
			m_slots[slotIndex][itemIndex]?.gameObject.SetActive(!value);
		}

		public void SetPause(bool pause)
		{
			for (int i = 0; i < m_slots.Length; i++)
			{
				m_slots[i].SetPause(pause);
			}
		}

		public bool TryGetItem(int slotIndex, int itemIndex, out StockViewItemData stockViewItemData)
		{
			stockViewItemData = default;

			if (_model[slotIndex][itemIndex].Quantity > 0)
			{
				stockViewItemData = ((IStockable)m_slots[slotIndex][itemIndex])?.StockViewItemData ?? new StockViewItemData
				{
					item = _model[slotIndex][itemIndex],
					stockView = this,
					slotIndex = slotIndex,
					itemIndex = itemIndex
				};

				return true;
			}

			return false;
		}

		public StockTransferData TransferData(StockViewItemData stockViewItemData)
		{
			return m_slots[stockViewItemData.slotIndex].TransferData;
		}

		public bool TryTransferAdd(StockItem stockItem, out StockViewItemData stockViewItemData, out Transform transform)
		{
			stockViewItemData = default;
			transform = null;

			if (_model.TryAdd(stockItem, out int slotIndex, out int itemIndex, true))
			{
				stockViewItemData = ((IStockable)m_slots[slotIndex][itemIndex])?.StockViewItemData ?? new StockViewItemData
				{
					item = stockItem,
					stockView = this,
					slotIndex = slotIndex,
					itemIndex = itemIndex
				};
				transform = m_slots[stockViewItemData.slotIndex].GetTransform(stockViewItemData.itemIndex);

				return true;
			}

			return false;
		}

		public bool TryTransferTake(StockViewItemData stockViewItemData, out Transform transform)
		{
			transform = m_slots[stockViewItemData.slotIndex].GetTransform(stockViewItemData.itemIndex);
			
			if (_model.TryTake(stockViewItemData.slotIndex, stockViewItemData.itemIndex, 1))
			{
				return true;
			}

			return false;
		}

		public void TransferDone(StockViewItemData stockViewItemData)
		{
			_model.TryInactivateItem(stockViewItemData.slotIndex, stockViewItemData.itemIndex, false);
		}

		public void TransferReject(StockViewItemData stockViewItemData)
		{
			// TODO reject item behaviour...
		}
	}
}