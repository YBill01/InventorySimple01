using GameName.Data;
using GameName.Gameplay.Features.Stock;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace GameName.UI
{
	public class UIPlayerStockSlot : MonoBehaviour, IDisposable
	{
		public event Action<int, int> OnSelectedItem;
		public event Action<PointerEventData> OnDragItem;
		public event Action<int, int> OnBeginDragItem;
		public event Action<PointerEventData, int, int> OnEndDragItem;

		[SerializeField]
		private UIPlayerStockSlotItem[] m_items;

		private StockSlotConfigData _config;
		public StockSlotConfigData Config => _config;

		public int SlotIndex { get; private set; }
		
		private StockSlot<StockItem> _modelSlot;

		private SharedData _sharedData;

		private UIPlayerStockPanel _panel;

		public void Init(StockSlotConfigData config, StockSlot<StockItem> modelSlot, int slotIndex, SharedData sharedData, UIPlayerStockPanel panel)
		{
			_config = config;

			_modelSlot = modelSlot;
			SlotIndex = slotIndex;

			_sharedData = sharedData;

			_panel = panel;

			for (int i = 0; i < m_items.Length; i++)
			{
				m_items[i].Init(_modelSlot, _sharedData, SlotIndex, i, _panel);

				m_items[i].OnSelectedItem += ItemsOnSelectedItem;
				m_items[i].OnDragItem += ItemsOnDragItem;
				m_items[i].OnBeginDragItem += ItemsOnBeginDragItem;
				m_items[i].OnEndDragItem += ItemsOnEndDragItem;
			}
		}

		public void Filling()
		{
			for (int i = 0; i < m_items.Length; i++)
			{
				m_items[i].Fill(_modelSlot[i]);
			}
		}

		public void Clear()
		{
			foreach (UIPlayerStockSlotItem item in m_items)
			{
				item.Clear();
			}
		}

		public void ItemsUpdate()
		{
			Clear();
			Filling();
		}

		private void ItemsOnSelectedItem(int itemIndex)
		{
			OnSelectedItem(itemIndex != -1 ? SlotIndex : -1, itemIndex);
		}
		private void ItemsOnDragItem(PointerEventData data)
		{
			OnDragItem(data);
		}
		private void ItemsOnBeginDragItem(int itemIndex)
		{
			OnBeginDragItem(SlotIndex, itemIndex);
		}
		private void ItemsOnEndDragItem(PointerEventData data, int itemIndex)
		{
			OnEndDragItem(data, SlotIndex, itemIndex);
		}

		public void Dispose()
		{
			foreach (UIPlayerStockSlotItem item in m_items)
			{
				item.OnSelectedItem -= ItemsOnSelectedItem;
				item.OnDragItem -= ItemsOnDragItem;
				item.OnBeginDragItem -= ItemsOnBeginDragItem;
				item.OnEndDragItem -= ItemsOnEndDragItem;
				item.Clear();
			}
		}
	}
}