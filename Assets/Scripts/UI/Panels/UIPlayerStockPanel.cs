using GameName.Data;
using GameName.Gameplay.Features.Stock;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameName.UI
{
	public class UIPlayerStockPanel : MonoBehaviour, IUIStockView
	{
		public event Action<int, int> OnDropItem;
		public event Action<int, int, int, int> OnTransferItem;

		[SerializeField]
		private UIPlayerStockSlot[] m_slots;

		[Space]
		[SerializeField]
		private RectTransform m_draggedItemContainer;

		[SerializeField]
		private UIPlayerStockSlotDraggedItem m_draggedItemPrefab;

		[NonSerialized]
		public UIPlayerStockSlotDraggedItem draggedItem;

		private StockConfigData _config;
		public StockConfigData Config => _config;

		private Stock _model;

		public Stock Stock => _model;

		private (int, int) _selectedItem = (-1, -1);


		private SharedData _sharedData;

		public void InitView(Stock model, SharedData sharedData)
		{
			_model = model;
			_config = _model.Config;

			_sharedData = sharedData;

			for (int i = 0; i < _config.slots.Length; i++)
			{
				m_slots[i].Init(_config.slots[i], _model[i], i, _sharedData, this);

				m_slots[i].OnSelectedItem += SlotsOnSelectedItem;
				m_slots[i].OnDragItem += SlotsOnDragItem;
				m_slots[i].OnBeginDragItem += SlotsOnBeginDragItem;
				m_slots[i].OnEndDragItem += SlotsOnEndDragItem;
			}

			Filling();
		}

		public void Filling()
		{
			foreach (UIPlayerStockSlot slot in m_slots)
			{
				slot.Filling();
			}
		}
		public void Clear()
		{
			foreach (UIPlayerStockSlot slot in m_slots)
			{
				slot.Clear();
			}
		}

		public void SlotsUpdate()
		{
			foreach (UIPlayerStockSlot slot in m_slots)
			{
				slot.ItemsUpdate();
			}
		}
		public void SlotUpdate(int slotIndex)
		{
			m_slots[slotIndex].ItemsUpdate();
		}

		private void SlotsOnSelectedItem(int slotIndex, int itemIndex)
		{
			_selectedItem = (slotIndex, itemIndex);
		}
		private void SlotsOnDragItem(PointerEventData data)
		{
			if (draggedItem != null)
			{
				if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_draggedItemContainer, data.position, data.pressEventCamera, out Vector3 globalMousePos))
				{
					var rt = draggedItem.GetComponent<RectTransform>();
					rt.position = globalMousePos;
				}
			}
		}
		private void SlotsOnBeginDragItem(int slotIndex, int itemIndex)
		{
			StockItem item = _model[slotIndex][itemIndex];

			draggedItem = Instantiate(m_draggedItemPrefab, m_draggedItemContainer);
			draggedItem.Init(item, slotIndex, itemIndex, _sharedData.GetItemData(item.id).view.icon);
		}
		private void SlotsOnEndDragItem(PointerEventData data, int slotIndex, int itemIndex)
		{
			if (draggedItem != null)
			{
				if (data.hovered.Contains(gameObject))
				{
					if (_selectedItem.Item1 != -1 && _selectedItem.Item2 != -1)
					{
						if (!(draggedItem.SlotIndex == _selectedItem.Item1 && draggedItem.ItemIndex == _selectedItem.Item2))
						{
							if (_model[_selectedItem.Item1].HasCondition(draggedItem.Item))
							{
								OnTransferItem(draggedItem.SlotIndex, draggedItem.ItemIndex, _selectedItem.Item1, _selectedItem.Item2);
							}
						}
					}
				}
				else
				{
					OnDropItem(slotIndex, itemIndex);
				}
				
				Destroy(draggedItem.gameObject);
				draggedItem = null;
			}
		}

		public void Dispose()
		{
			foreach (UIPlayerStockSlot slot in m_slots)
			{
				slot.OnSelectedItem -= SlotsOnSelectedItem;
				slot.OnDragItem -= SlotsOnDragItem;
				slot.OnBeginDragItem -= SlotsOnBeginDragItem;
				slot.OnEndDragItem -= SlotsOnEndDragItem;
				slot.Dispose();
			}
		}
	}
}