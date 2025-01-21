using GameName.Data;
using GameName.Gameplay.Features.Stock;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameName.UI
{
	public class UIPlayerStockSlotItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		public event Action<int> OnSelectedItem;
		public event Action<PointerEventData> OnDragItem;
		public event Action<int> OnBeginDragItem;
		public event Action<PointerEventData, int> OnEndDragItem;

		[SerializeField]
		private Image m_icon;

		[Space]
		[SerializeField]
		private TMP_Text m_quantityText;

		[Space]
		[SerializeField]
		private Image m_outlineSelected;
		[SerializeField]
		private Image m_outlineSelectedTransfer;

		public int SlotIndex { get; private set; }
		public int ItemIndex { get; private set; }

		private bool _isEmpty = true;
		public bool IsEmpty => _isEmpty;

		private SharedData _sharedData;

		private UIPlayerStockPanel _panel;

		private StockSlot<StockItem> _modelSlot;

		public void Init(StockSlot<StockItem> modelSlot, SharedData sharedData, int slotIndex, int itemIndex, UIPlayerStockPanel panel)
		{
			_modelSlot = modelSlot;

			_sharedData = sharedData;

			SlotIndex = slotIndex;
			ItemIndex = itemIndex;

			_panel = panel;

			Clear();
		}

		public void Fill(StockItem item)
		{
			if (item.Quantity > 0)
			{
				ItemData itemData = _sharedData.GetItemData(item.id);

				m_icon.sprite = itemData.view.icon;
				m_icon.gameObject.SetActive(true);

				m_quantityText.text = $"{item.Quantity}/{itemData.stackCapacity}";

				_isEmpty = false;

				Inactivate(item.inactive);
			}
			else
			{
				Clear();
			}
		}
		public void Clear()
		{
			m_icon.sprite = null;
			m_icon.gameObject.SetActive(false);
			m_quantityText.text = "";

			_isEmpty = true;

			m_outlineSelected.gameObject.SetActive(false);
			m_outlineSelectedTransfer.gameObject.SetActive(false);
		}

		public void Inactivate(bool value)
		{
			m_icon.color = value ? new Color(1.0f, 1.0f, 1.0f, 0.25f) : Color.white;
		}
		
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (_panel.draggedItem != null)
			{
				if (!(_panel.draggedItem.SlotIndex == SlotIndex && _panel.draggedItem.ItemIndex == ItemIndex))
				{
					if (_modelSlot.HasCondition(_panel.draggedItem.Item))
					{
						m_outlineSelectedTransfer.gameObject.SetActive(true);

						OnSelectedItem(ItemIndex);
					}
				}
			}
			else if (!_isEmpty)
			{
				m_outlineSelected.gameObject.SetActive(true);

				OnSelectedItem(ItemIndex);
			}
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			m_outlineSelected.gameObject.SetActive(false);
			m_outlineSelectedTransfer.gameObject.SetActive(false);

			OnSelectedItem(-1);
		}

		public void OnDrag(PointerEventData eventData)
		{
			OnDragItem(eventData);
		}
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (!_isEmpty)
			{
				Inactivate(true);

				m_outlineSelected.gameObject.SetActive(false);
				m_outlineSelectedTransfer.gameObject.SetActive(false);

				OnBeginDragItem(ItemIndex);
			}
		}
		public void OnEndDrag(PointerEventData eventData)
		{
			Inactivate(_modelSlot[ItemIndex].inactive);

			OnEndDragItem(eventData, ItemIndex);
		}
	}
}